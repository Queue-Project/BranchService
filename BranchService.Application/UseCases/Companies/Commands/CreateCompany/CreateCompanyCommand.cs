using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.Companies.Commands.CreateCompany;

public record CreateCompanyCommand(
    string CompanyName, 
    string Address,
    string EmailAddress,
    string PhoneNumber)
    : IRequest<CompanyResponseModel>;