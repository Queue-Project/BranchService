using BranchService.API.Controllers;
using BranchService.Application.Response;
using BranchService.Application.UseCases.CompanyServices.Commands.CreateService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyServiceControllerTests;

public class CreateCompanyServiceEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyServiceController>> _mockLogger;
    private readonly CompanyServiceController _companyServiceController;

    public CreateCompanyServiceEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyServiceController>>();
        _companyServiceController = new CompanyServiceController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_CreatedCompanyService_WithOkStatusCode()
    {
        // Arrange
        var createCompanyServiceCommand = new CreateServiceCommand
        (
            1,
            1,
            "Test Service Name",
            "Test Service Description",
            45
        );

        var expectedResponse = new CompanyServiceResponseModel()
        {
            Id = 1,
            CompanyId = 1,
            BranchId = 1,
            ServiceName = "Test Service Name",
            ServiceDescription = "Test Service Description",
            ServiceDuration = 45,
        };

        _mockMediator
            .Setup(m => m.Send(createCompanyServiceCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _companyServiceController.PostAsync(createCompanyServiceCommand);

        // Assert
        var okResult = result.ShouldBeOfType<OkObjectResult>();
        var returnValue = okResult.Value.ShouldBeOfType<CompanyServiceResponseModel>();

        returnValue.Id.ShouldBe(expectedResponse.Id);
        returnValue.ServiceName.ShouldBe(expectedResponse.ServiceName);
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_InvalidCommand()
    {
        // Arrange
        var createCompanyServiceCommand = new CreateServiceCommand
        (
            1,
            1,
            "",
            "",
            45
        );

        _mockMediator
            .Setup(m => m.Send(createCompanyServiceCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));


        //Act
        var result = _companyServiceController.PostAsync(createCompanyServiceCommand);

        //Assert
        var exception = result.ShouldThrow<FluentValidation.ValidationException>();
        exception.Message.ShouldBe("Validation failed");
    }
}