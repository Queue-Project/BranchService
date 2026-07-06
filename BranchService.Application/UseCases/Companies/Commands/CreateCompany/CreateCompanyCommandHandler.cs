using BranchService.Application.Interfaces.Data;
using BranchService.Application.Response;
using BranchService.Contracts.Events;
using BranchService.Contracts.Events.CompanyEvents;
using BranchService.Domain.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BranchService.Application.UseCases.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CompanyResponseModel>
{
    private readonly ILogger<CreateCompanyCommandHandler> _logger;
    private readonly IBranchServiceApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;


    public CreateCompanyCommandHandler(ILogger<CreateCompanyCommandHandler> logger,
        IBranchServiceApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CompanyResponseModel> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new company with Name {companyName}", request.CompanyName);

        var company = new CompanyEntity()
        {
            CompanyName = request.CompanyName,
            Address = request.Address,
            EmailAddress = request.EmailAddress,
            PhoneNumber = request.PhoneNumber,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _dbContext.Companies.AddAsync(company, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Company {companyName} added successfully with Id {companyId}", company.CompanyName,
            company.Id);

        await _publishEndpoint.Publish(new CompanyCreatedEvent
        {
            OccuredAt = DateTimeOffset.UtcNow,
            CompanyId = company.Id,
            CompanyName = company.CompanyName,
            Address = company.Address,
            EmailAddress = company.EmailAddress,
            PhoneNumber = company.PhoneNumber,
            AuditData = new AuditData
            {
                PerformedByUserId = 1,
                PerformedByUserName = "systemAdmin"
            }
        }, cancellationToken);

        var response = new CompanyResponseModel
        {
            Id = company.Id,
            CompanyName = company.CompanyName,
            Address = company.Address,
            EmailAddress = company.EmailAddress,
            PhoneNumber = company.PhoneNumber,
            CreatedAt = company.CreatedAt
        };

        return response;
    }
}