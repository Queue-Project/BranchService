using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.Companies.Queries.GetAllCompanies;

public record GetAllCompaniesQuery(int PageNumber): IRequest<PagedResponse<CompanyResponseModel>>;