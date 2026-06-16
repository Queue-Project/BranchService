using MediatR;

namespace BranchService.Application.UseCases.CompanyServices.Commands.DeleteService;

public record DeleteServiceCommand(int Id): IRequest<bool>;