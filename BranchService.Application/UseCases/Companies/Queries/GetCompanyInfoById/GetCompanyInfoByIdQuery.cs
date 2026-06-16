using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.Companies.Queries.GetCompanyInfoById;

public record GetCompanyInfoByIdQuery(int Id) : IRequest<CompanyByIdResponseModel>; 