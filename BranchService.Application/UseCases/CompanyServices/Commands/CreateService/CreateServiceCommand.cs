using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.CompanyServices.Commands.CreateService;

public record CreateServiceCommand(int CompanyId, int BranchId, string ServiceName, string ServiceDescription, int ServiceDuration): IRequest<CompanyServiceResponseModel>;