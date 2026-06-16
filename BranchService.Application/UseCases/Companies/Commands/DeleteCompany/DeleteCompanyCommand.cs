using MediatR;

namespace BranchService.Application.UseCases.Companies.Commands.DeleteCompany;

public record DeleteCompanyCommand(int Id): IRequest<bool>;