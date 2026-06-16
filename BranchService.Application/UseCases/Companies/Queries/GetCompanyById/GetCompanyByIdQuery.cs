using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.Companies.Queries.GetCompanyById;

public record GetCompanyByIdQuery(int Id): IRequest<CompanyResponseModel>;