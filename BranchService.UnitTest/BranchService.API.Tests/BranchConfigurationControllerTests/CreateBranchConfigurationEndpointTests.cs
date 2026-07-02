using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Response;
using BranchService.Application.UseCases.BranchConfigurations.Commands.CreateBranchConfiguration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchConfigurationControllerTests;

public class CreateBranchConfigurationEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchConfigurationController>> _mockLogger;
    private readonly BranchConfigurationController _branchConfigurationController;

    public CreateBranchConfigurationEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchConfigurationController>>();
        _branchConfigurationController = new BranchConfigurationController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_CreatedBranchConfiguration_WithOkStatusCode()
    {
        // Arrange
        var createBranchConfigurationCommand = new CreateBranchConfigurationCommand
        (
            1,
            50,
            new TimeOnly(09, 00, 00),
            new TimeOnly(19, 00, 00),
            new TimeOnly(13, 00, 00),
            new TimeOnly(14, 00, 00)
        );

        var expectedResponse = new BranchConfigurationResponseModel
        {
            Id = 1,
            BranchId = 1,
            MaxTicketsPerDay = 50,
            OpenTime = new TimeOnly(09, 00, 00),
            CloseTime =   new TimeOnly(19, 00, 00), 
            BreakStartTime = new TimeOnly(13, 00, 00),
            BreakEndTime = new TimeOnly(14, 00, 00)
        };

        _mockMediator
            .Setup(m => m.Send(createBranchConfigurationCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _branchConfigurationController.PostAsync(createBranchConfigurationCommand);

        // Assert
        var okResult = result.ShouldBeOfType<OkObjectResult>();
        var returnValue = okResult.Value.ShouldBeOfType<BranchConfigurationResponseModel>();

        returnValue.Id.ShouldBe(expectedResponse.Id);
        returnValue.BranchId.ShouldBe(expectedResponse.BranchId);
        returnValue.MaxTicketsPerDay.ShouldBe(expectedResponse.MaxTicketsPerDay);
    }
    

    [Fact]
    public async Task Should_Return_BranchNotFound()
    {
        //Arrange
        var createBranchConfigurationCommand = new CreateBranchConfigurationCommand
        (
            1,
            50,
            new TimeOnly(09, 00, 00),
            new TimeOnly(19, 00, 00),
            new TimeOnly(13, 00, 00),
            new TimeOnly(14, 00, 00)
        );

        var expectedResponse = new HttpStatusCodeException(HttpStatusCode.NotFound, "Branch not found");

        _mockMediator.Setup(s => s.Send(createBranchConfigurationCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedResponse);

        //Act
        var result = _branchConfigurationController.PostAsync(createBranchConfigurationCommand);

        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Branch not found");
    }
}