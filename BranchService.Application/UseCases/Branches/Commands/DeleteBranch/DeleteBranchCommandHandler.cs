using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.Interfaces.Data;
using BranchService.Contracts.Events;
using BranchService.Contracts.Events.BranchEvents;
using BranchService.Contracts.Events.Enums;
using BranchService.Domain.Models;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BranchService.Application.UseCases.Branches.Commands.DeleteBranch;

public class DeleteBranchCommandHandler: IRequestHandler<DeleteBranchCommand, bool>
{
    private readonly ILogger<DeleteBranchCommandHandler> _logger;
    private readonly IBranchServiceApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteBranchCommandHandler(ILogger<DeleteBranchCommandHandler> logger, IBranchServiceApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting branch with Id {branchId}", request.Id);
        var branch = await _dbContext.Branches.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (branch==null)
        {
            _logger.LogInformation("Branch with Id {branchId} not found for updating", request.Id);
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, nameof(BranchEntity));
        }
        var dbCompany = await _dbContext.Companies.FirstOrDefaultAsync(s => s.Id == branch.CompanyId, cancellationToken);
        

        branch.IsActive = false;

        await _publishEndpoint.Publish(new BranchDeletedEvent
        {
            OccuredAt = DateTimeOffset.UtcNow,
            BranchId = branch.Id,
            CompanyId = branch.CompanyId,
            CompanyCategory = (CompanyCategory) dbCompany!.CompanyCategory,
            BranchName = branch.BranchName,
            City = branch.City,
            Address = branch.Address,
            EmailAddress = branch.EmailAddress,
            PhoneNumber = branch.PhoneNumber,
            IsActive = branch.IsActive,
            AuditData = new AuditData
            {
                PerformedByUserId = 1,
                PerformedByUserName = "systemAdmin"
            }
        }, cancellationToken);

        return true;
    }
}