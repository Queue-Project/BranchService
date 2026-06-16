using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.Companies.Commands.UpdateCompany;

public record UpdateCompanyCommand(int Id,string CompanyName, string Address, string EmailAddress, string PhoneNumber) : IRequest<CompanyResponseModel>;
