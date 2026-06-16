using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.CompanyServices.Commands.CreateService;

public record CreateServiceCommand(int CompanyId, string ServiceName, string ServiceDescription): IRequest<CompanyServiceResponseModel>;