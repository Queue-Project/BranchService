using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.BranchConfigurations.Commands.UpdateBranchConfiguration;

public record UpdateBranchConfigurationCommand(
    int Id,
    int MaxTicketsPerDay,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    TimeOnly? BreakStartTime,
    TimeOnly? BreakEndTime): IRequest<BranchConfigurationResponseModel>;