using BranchService.Application.Response;
using MediatR;

namespace BranchService.Application.UseCases.CompanyServices.Commands.UpdateService;

public record UpdateServiceCommand(int Id,int CompanyId, string ServiceName, string ServiceDescription): IRequest<CompanyServiceResponseModel>;