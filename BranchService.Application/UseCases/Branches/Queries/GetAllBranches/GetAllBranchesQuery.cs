using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.Branches.Queries.GetAllBranches;

public record GetAllBranchesQuery(int PageNumber): IRequest<PagedResponse<BranchResponseModel>>;