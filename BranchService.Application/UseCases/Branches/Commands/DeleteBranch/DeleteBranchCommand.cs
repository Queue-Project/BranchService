using MediatR;

namespace BranchService.Application.UseCases.Branches.Commands.DeleteBranch;

public record DeleteBranchCommand(int Id): IRequest<bool>;