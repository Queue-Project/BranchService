using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.BranchConfigurations.Queries.GetBranchConfigurationById;

public record GetBranchConfigurationByIdQuery(int Id): IRequest<BranchConfigurationResponseModel>;