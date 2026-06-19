using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Requests;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Companies.Commands.UpdateCompany;
using BranchService.Application.UseCases.CompanyServices.Commands.UpdateService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyServiceControllerTests;

public class UpdateCompanyServiceEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyServiceController>> _mockLogger;
    private readonly CompanyServiceController _companyServiceController;

    public UpdateCompanyServiceEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyServiceController>>();
        _companyServiceController = new CompanyServiceController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_UpdatedCompanyService_WithOkStatusCode()
    {
        //Arrange
        int id = 1;
        var updateRequest = new UpdateCompanyServiceRequest
        {
            CompanyId = 1,
            ServiceName = "Update Company Service Name",
            ServiceDescription = "Update Company Service Description"
        };


        var expectedResponse = new CompanyServiceResponseModel
        {
            Id = 1,
            CompanyId = 1,
            ServiceName = "Update Company Service Name",
            ServiceDescription = "Update Company Service Description"
        };

        var updateCommand = new UpdateServiceCommand(id,
            updateRequest.CompanyId,
            updateRequest.ServiceName,
            updateRequest.ServiceDescription);

        _mockMediator.Setup(s => s.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        //Act
        var result = await _companyServiceController.Put(id, updateRequest);


        //Assert
        result.ShouldNotBeNull();
        var statusCode = result.ShouldBeOfType<OkObjectResult>();
        var returnValue = statusCode.Value.ShouldBeOfType<CompanyServiceResponseModel>();

        returnValue.Id.ShouldBe(id);
        returnValue.ServiceName.ShouldBe(expectedResponse.ServiceName);
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_InvalidCommand()
    {
        // Arrange

        int id = 1;
        var updateRequest = new UpdateCompanyServiceRequest
        {
            CompanyId = 1,
            ServiceName = "Update Company Service Name",
            ServiceDescription = "Update Company Service Description"
        };
        

        var updateCommand = new UpdateServiceCommand(id,
            updateRequest.CompanyId,
            updateRequest.ServiceName,
            updateRequest.ServiceDescription);


        _mockMediator
            .Setup(m => m.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));


        //Act
        var result = _companyServiceController.Put(id, updateRequest);

        //Assert
        var exception = result.ShouldThrow<FluentValidation.ValidationException>();
        exception.Message.ShouldBe("Validation failed");
    }

    [Fact]
    public async Task Should_Return_NotFound_When_CompanyServiceDoesNotExist()
    {
        int id = 999;
        var updateRequest = new UpdateCompanyServiceRequest
        {
            CompanyId = 1,
            ServiceName = "Update Company Service Name",
            ServiceDescription = "Update Company Service Description"
        };
        

        var updateCommand = new UpdateServiceCommand(id,
            updateRequest.CompanyId,
            updateRequest.ServiceName,
            updateRequest.ServiceDescription);

        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "CompanyService not found");

        _mockMediator
            .Setup(s => s.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result = _companyServiceController.Put(id, updateRequest);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();

        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("CompanyService not found");
    }
}