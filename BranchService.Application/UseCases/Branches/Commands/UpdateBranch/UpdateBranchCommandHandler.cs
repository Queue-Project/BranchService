using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.Helpers;
using BranchService.Application.Interfaces.Data;
using BranchService.Application.Response;
using BranchService.Contracts.Events;
using BranchService.Contracts.Events.BranchEvents;
using BranchService.Domain.Models;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BranchService.Application.UseCases.Branches.Commands.UpdateBranch;

public class UpdateBranchCommandHandler: IRequestHandler<UpdateBranchCommand, BranchResponseModel>
{
    private readonly ILogger<UpdateBranchCommandHandler> _logger;
    private readonly IBranchServiceApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateBranchCommandHandler(ILogger<UpdateBranchCommandHandler> logger, IBranchServiceApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<BranchResponseModel> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating branch with Id {branchId}", request.Id);
        var branch = await _dbContext.Branches.FirstOrDefaultAsync(s => s.Id == request.Id && s.IsActive == true, cancellationToken);
        if (branch== null)
        {
            _logger.LogInformation("Branch with Id {branchId} not found for updating", request.Id);
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, nameof(BranchEntity));
        }

        branch.BranchName = request.BranchName;
        branch.City = request.City;
        branch.Address = request.Address;
        branch.EmailAddress = request.EmailAddress;
        branch.PhoneNumber = request.PhoneNumber;

        var entry = _dbContext.Entry(branch);
        var changes = AuditHelper.GetChanges(entry);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Branch with Id {branchId} updated successfully", request.Id);

        
        
        await _publishEndpoint.Publish(new BranchUpdatedEvent
        {
            OccuredAt = DateTimeOffset.UtcNow,
            BranchId = branch.Id,
            CompanyId = branch.CompanyId,
            BranchName = branch.BranchName,
            City = branch.City,
            Address = branch.Address,
            EmailAddress = branch.EmailAddress,
            PhoneNumber = branch.PhoneNumber,
            IsActive = branch.IsActive,
            AuditData = new AuditData
            {
                PerformedByUserId = 1,
                PerformedByUserName = "systemAdmin",
                Changes = changes
            }
        }, cancellationToken);
        
        var response = new BranchResponseModel
        {
            Id = branch.Id,
            CompanyId = branch.CompanyId,
            BranchName = branch.BranchName,
            City = branch.City,
            Address = branch.Address,
            EmailAddress = branch.EmailAddress,
            PhoneNumber = branch.PhoneNumber,
            CreatedAt = branch.CreatedAt,
            IsActive = branch.IsActive
        };

        return response;

    }
}