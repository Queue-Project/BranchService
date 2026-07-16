using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.Helpers;
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

namespace BranchService.Application.UseCases.CompanyServices.Commands.UpdateService;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, CompanyServiceResponseModel>
{
    private readonly ILogger<UpdateServiceCommandHandler> _logger;
    private readonly IBranchServiceApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateServiceCommandHandler(ILogger<UpdateServiceCommandHandler> logger,
        IBranchServiceApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CompanyServiceResponseModel> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating service with Id {id}.", request.Id);
        
        var dbService = await _dbContext.CompanyServices.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (dbService == null)
        {
            _logger.LogWarning("Service with Id {id} not found for updating.", request.Id);
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, nameof(CompanyServiceEntity));
        }

        var dbCompany = await _dbContext.Companies.FirstOrDefaultAsync(s => s.Id == dbService.CompanyId, cancellationToken);

        dbService.ServiceName = request.ServiceName;
        dbService.ServiceDescription = request.ServiceDescription;
        dbService.ServiceDuration = request.ServiceDuration;
        
        
        var entry = _dbContext.Entry(dbService);
        var changes = AuditHelper.GetChanges(entry);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Service with Id {id} updated successfully.", request.Id);

        
        
        await _publishEndpoint.Publish(new CompanyServiceUpdatedEvent
        {
            OccuredAt = DateTimeOffset.UtcNow,
            CompanyServiceId = dbService.Id,
            CompanyId = dbService.CompanyId,
            CompanyCategory =(CompanyCategory)dbCompany!.CompanyCategory,
            BranchId = dbService.BranchId,
            ServiceDescription = dbService.ServiceDescription,
            ServiceName = dbService.ServiceName,
            ServiceDuration = dbService.ServiceDuration,
            AuditData = new AuditData
            {
                PerformedByUserId = 1,
                PerformedByUserName = "systemAdmin",
                Changes = changes
            }
        }, cancellationToken);
        
        var response = new CompanyServiceResponseModel()
        {
            Id = dbService.Id,
            CompanyId = dbService.CompanyId,
            BranchId = dbService.BranchId,
            ServiceName = dbService.ServiceName,
            ServiceDescription = dbService.ServiceDescription,
            ServiceDuration = dbService.ServiceDuration,
        };

        return response;
    }
}