using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.CompanyServices.Queries.GetAllServices;

public record GetAllServicesQuery(int PageNumber): IRequest<PagedResponse<CompanyServiceResponseModel>>;