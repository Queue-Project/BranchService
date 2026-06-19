using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.Interfaces.Data;
using BranchService.Application.Response;
using BranchService.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BranchService.Application.UseCases.BranchConfigurations.Commands.CreateBranchConfiguration;

public class CreateBranchConfigurationCommandHandler: IRequestHandler<CreateBranchConfigurationCommand, BranchConfigurationResponseModel>
{
    private readonly ILogger<CreateBranchConfigurationCommandHandler> _logger;
    private readonly IBranchServiceApplicationDbContext _dbContext;

    public CreateBranchConfigurationCommandHandler(ILogger<CreateBranchConfigurationCommandHandler> logger, IBranchServiceApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<BranchConfigurationResponseModel> Handle(CreateBranchConfigurationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating configurations for BranchId: {BranchId}", request.BranchId);

        var branch = await _dbContext.Branches
            .FirstOrDefaultAsync(s => s.Id == request.BranchId, cancellationToken);

        if (branch== null)
        {
            _logger.LogError("Branch with Id {BranchId} not found ", request.BranchId);
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Branch with Id {request.BranchId} not found");
        }
        
        
        var branchConfiguration = new BranchConfigurationEntity
        {
            BranchId = request.BranchId,
            MaxTicketsPerDay = request.MaxTicketsPerDay,
            OpenTime = request.OpenTime,
            CloseTime = request.CloseTime,
            BreakStartTime = request.BreakStartTime,
            BreakEndTime = request.BreakEndTime,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _dbContext.BranchConfigurations.AddAsync(branchConfiguration, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created branch configuration with Id: {branchId}", branchConfiguration.Id);
        
        var response = new BranchConfigurationResponseModel
        {
            Id = branchConfiguration.Id,
            BranchId = branchConfiguration.BranchId,
            MaxTicketsPerDay = branchConfiguration.MaxTicketsPerDay,
            OpenTime = branchConfiguration.OpenTime,
            CloseTime = branchConfiguration.CloseTime,
            BreakStartTime = branchConfiguration.BreakStartTime,
            BreakEndTime = branchConfiguration.BreakEndTime,
            CreatedAt = branchConfiguration.CreatedAt
        };

        return response;
    }
}