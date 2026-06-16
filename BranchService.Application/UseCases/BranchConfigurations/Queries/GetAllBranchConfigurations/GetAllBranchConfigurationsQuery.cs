using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.BranchConfigurations.Queries.GetAllBranchConfigurations;

public record GetAllBranchConfigurationsQuery(int PageNumber): IRequest<PagedResponse<BranchConfigurationResponseModel>>;