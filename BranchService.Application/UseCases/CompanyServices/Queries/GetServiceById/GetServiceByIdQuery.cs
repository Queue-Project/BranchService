using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.CompanyServices.Queries.GetServiceById;

public record GetServiceByIdQuery(int Id): IRequest<CompanyServiceResponseModel>;