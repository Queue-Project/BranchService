using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.BranchConfigurations.Commands.CreateBranchConfiguration;

public record CreateBranchConfigurationCommand(
    int BranchId, 
    int MaxTicketsPerDay, 
    TimeOnly OpenTime, 
    TimeOnly CloseTime,
    TimeOnly? BreakStartTime,
    TimeOnly? BreakEndTime)
    : IRequest<BranchConfigurationResponseModel>;