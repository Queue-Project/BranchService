using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.Interfaces.Data;
using BranchService.Application.Response;
using BranchService.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BranchService.Application.UseCases.CompanyServices.Queries.GetServiceById;

public class GetServiceByIdQueryHandler: IRequestHandler<GetServiceByIdQuery, CompanyServiceResponseModel>
{
    private readonly ILogger<GetServiceByIdQueryHandler> _logger;
    private readonly IBranchServiceApplicationDbContext _dbContext;

    public GetServiceByIdQueryHandler(ILogger<GetServiceByIdQueryHandler> logger, IBranchServiceApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<CompanyServiceResponseModel> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting service by Id {id}", request.Id);
        var dbService = await _dbContext.CompanyServices.FirstOrDefaultAsync(s=>s.Id== request.Id, cancellationToken);
        if (dbService == null)
        {
            _logger.LogWarning("Service with Id {id} not found.", request.Id);
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, nameof(CompanyServiceEntity));
        }

        var response = new CompanyServiceResponseModel()
        {
            Id = dbService.Id,
            CompanyId = dbService.CompanyId,
            BranchId = dbService.BranchId,
            ServiceName = dbService.ServiceName,
            ServiceDescription = dbService.ServiceDescription,
            ServiceDuration = dbService.ServiceDuration
        };

        _logger.LogInformation("Service with Id {id} fetched successfully.", request.Id);
        return response;
    }
}