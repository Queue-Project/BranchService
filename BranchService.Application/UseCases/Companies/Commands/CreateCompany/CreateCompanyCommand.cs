using BranchService.Application.Response;
using BranchService.Domain.Enums;
using MediatR;

namespace BranchService.Application.UseCases.Companies.Commands.CreateCompany;

public record CreateCompanyCommand(
    string CompanyName, 
    string Address,
    string EmailAddress,
    string PhoneNumber,
    CompanyCategory CompanyCategory)
    : IRequest<CompanyResponseModel>;