using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.CompanyServices.Commands.UpdateService;

public record UpdateServiceCommand(int Id, string ServiceName, string ServiceDescription, int ServiceDuration): IRequest<CompanyServiceResponseModel>;