using MediatR;

namespace BranchService.Application.UseCases.BranchConfigurations.Commands.DeleteBranchConfiguration;

public record DeleteBranchConfigurationCommand(int Id): IRequest<bool>;