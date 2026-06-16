using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.Branches.Queries.GetBranchById;

public record GetBranchByIdQuery(int Id): IRequest<BranchResponseModel>;