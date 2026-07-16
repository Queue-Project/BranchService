using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.Interfaces.Data;
using BranchService.Application.Response;
using BranchService.Contracts.Events;
using BranchService.Contracts.Events.CompanyServiceEvents;
using BranchService.Contracts.Events.Enums;
using BranchService.Domain.Models;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BranchService.Application.UseCases.CompanyServices.Commands.CreateService;

public class CreateServiceCommandHandler: IRequestHandler<CreateServiceCommand, CompanyServiceResponseModel>
{
    private readonly ILogger<CreateServiceCommandHandler> _logger;
    private readonly IBranchServiceApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    

    public CreateServiceCommandHandler(ILogger<CreateServiceCommandHandler> logger, IBranchServiceApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CompanyServiceResponseModel> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new service with Name {request.ServiceName}", request.ServiceName);

        
        var dbCompany = await _dbContext.Companies.FirstOrDefaultAsync(s => s.Id == request.CompanyId, cancellationToken);
        if (dbCompany== null)
        {
            _logger.LogError("Company with Id {CompanyId} not found", request.CompanyId);
            throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                $"Company with Id {request.CompanyId} not found");
        }
        
        var dbBranch = await _dbContext.Branches.FirstOrDefaultAsync(s => s.Id == request.BranchId, cancellationToken);
        if (dbBranch== null)
        {
            _logger.LogError("Branch with Id {BranchId} not found", request.BranchId);
            throw new HttpStatusCodeException(HttpStatusCode.NotFound,
                $"Branch with Id {request.BranchId} not found");
        }
        
        var service = new CompanyServiceEntity
        {
            CompanyId = request.CompanyId,
            BranchId = request.BranchId,
            ServiceName = request.ServiceName,
            ServiceDescription = request.ServiceDescription,
            ServiceDuration = request.ServiceDuration,
            CreatedAt = DateTimeOffset.UtcNow
        };
        
        
        

        await _dbContext.CompanyServices.AddAsync(service, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Service {service.ServiceName} added successfully with Id {service.Id}.",
            service.ServiceName, service.Id);

        await _publishEndpoint.Publish(new CompanyServiceCreatedEvent
        {
            OccuredAt = DateTimeOffset.UtcNow,
            CompanyServiceId = service.Id,
            CompanyId = service.CompanyId,
            CompanyCategory = (CompanyCategory)dbCompany.CompanyCategory,
            BranchId = service.BranchId,
            ServiceDescription = service.ServiceDescription,
            ServiceName = service.ServiceName,
            ServiceDuration = service.ServiceDuration,
            AuditData = new AuditData
            {
                PerformedByUserId = 1,
                PerformedByUserName = "systemAdmin"
            }
        }, cancellationToken);
        
        var response = new CompanyServiceResponseModel()
        {
            Id = service.Id,
            CompanyId = service.CompanyId,
            BranchId = service.BranchId,
            ServiceName = service.ServiceName,
            ServiceDescription = service.ServiceDescription,
            ServiceDuration = service.ServiceDuration
        };

        return response;
    }
}