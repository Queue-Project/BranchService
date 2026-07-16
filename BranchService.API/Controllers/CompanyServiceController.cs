using BranchService.Application.Requests;
using BranchService.Application.Response;
using BranchService.Application.UseCases.CompanyServices.Commands.CreateService;
using BranchService.Application.UseCases.CompanyServices.Commands.DeleteService;
using BranchService.Application.UseCases.CompanyServices.Commands.UpdateService;
using BranchService.Application.UseCases.CompanyServices.Queries.GetAllServices;
using BranchService.Application.UseCases.CompanyServices.Queries.GetServiceById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BranchService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyServiceController : ControllerBase
{
    private readonly ILogger<CompanyServiceController> _logger;
    private readonly IMediator _mediator;

    public CompanyServiceController(ILogger<CompanyServiceController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<CompanyServiceResponseModel>>> GetAllAsync([FromQuery]int pageNumber=1)
    {
        _logger.LogInformation("Received request to get all services. PageNumber: {PageNumber}, PageSize: 15",
            pageNumber);
        var query = new GetAllServicesQuery(pageNumber);
        var services = await _mediator.Send(query);
        return Ok(services);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyServiceResponseModel>> GetByIdAsync([FromRoute] int id)
    {
        _logger.LogInformation("Received request to get service with Id: {serviceId}", id);
        var query = new GetServiceByIdQuery(id);
        var service = await _mediator.Send(query);
        _logger.LogInformation("Successfully returned service with Id: {serviceId}", id);
        return Ok(service);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateServiceCommand request)
    {
        _logger.LogInformation("Received request to create service. ServiceName: {serviceName}", request.ServiceName);
        var service = await _mediator.Send(request);
        _logger.LogInformation("Successfully created service with Id: {serviceId}", service.Id);
        return Ok(service);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateCompanyServiceRequest request)
    {
        _logger.LogInformation("Received request to update service with Id: {serviceId}", id);
        var command = new UpdateServiceCommand(id, request.ServiceName, request.ServiceDescription, request.ServiceDuration);
        var update = await _mediator.Send(command);
        _logger.LogInformation("Successfully updated service with Id: {serviceId}", id);
        return Ok(update);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        _logger.LogInformation("Received request to delete service with Id: {serviceId}", id);
        var command = new DeleteServiceCommand(id);
        await _mediator.Send(command);
        _logger.LogInformation("Successfully deleted service with Id: {serviceId}", id);
        return NoContent();
    }
}