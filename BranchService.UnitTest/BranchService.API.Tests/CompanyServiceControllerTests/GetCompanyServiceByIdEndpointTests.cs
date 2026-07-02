using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Response;
using BranchService.Application.UseCases.CompanyServices.Queries.GetServiceById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyServiceControllerTests;

public class GetCompanyServiceByIdEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyServiceController>> _mockLogger;
    private readonly CompanyServiceController _companyServiceController;

    public GetCompanyServiceByIdEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyServiceController>>();
        _companyServiceController = new CompanyServiceController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_CompanyById_WithOkStatusCode()
    {
        var companyId = 1;
        var query = new GetServiceByIdQuery(companyId);

        var expectedResponse = new CompanyServiceResponseModel()
        {
            Id = 1,
            CompanyId = 1,
            ServiceName = "Test Service Name",
            ServiceDescription = "Test Service Description"
        };

        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _companyServiceController.GetByIdAsync(companyId);

        // Assert
        result.ShouldNotBeNull();
        
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(200);
        
        var returnedValue = okResult.Value.ShouldBeOfType<CompanyServiceResponseModel>();
        returnedValue.Id.ShouldBe(expectedResponse.Id);
        returnedValue.ServiceName.ShouldBe(expectedResponse.ServiceName);
        returnedValue.ServiceDescription.ShouldBe(expectedResponse.ServiceDescription);

    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_CompanyDoesNotExist()
    {
        var companyId = 999;
        var query = new GetServiceByIdQuery(companyId);
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Company Service not found");
    
        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _companyServiceController.GetByIdAsync(companyId);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Company Service not found");

    }
}