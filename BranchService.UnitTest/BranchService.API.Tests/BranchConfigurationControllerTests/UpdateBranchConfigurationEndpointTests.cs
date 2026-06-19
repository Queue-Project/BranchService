using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Requests;
using BranchService.Application.Response;
using BranchService.Application.UseCases.BranchConfigurations.Commands.UpdateBranchConfiguration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchConfigurationControllerTests;

public class UpdateBranchConfigurationEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchConfigurationController>> _mockLogger;
    private readonly BranchConfigurationController _branchConfigurationController;

    public UpdateBranchConfigurationEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchConfigurationController>>();
        _branchConfigurationController = new BranchConfigurationController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_UpdatedBranchConfiguration_WithOkStatusCode()
    {
        //Arrange
        int id = 1;
        var updateRequest = new UpdateBranchConfigurationRequest()
        {
            MaxTicketsPerDay = 50,
            OpenTime = new TimeOnly(08,00,00),
            CloseTime = new TimeOnly(18,00,00),
            BreakStartTime = new TimeOnly(13,00,00),
            BreakEndTime = new TimeOnly(14,00,00)
        };


        var expectedResponse = new BranchConfigurationResponseModel
        {
            Id = 1,
            BranchId = 1,
            MaxTicketsPerDay = 50,
            OpenTime = new TimeOnly(08, 00, 00),
            CloseTime =   new TimeOnly(18, 00, 00), 
            BreakStartTime = new TimeOnly(13, 00, 00),
            BreakEndTime = new TimeOnly(14, 00, 00)
        };

        var updateCommand = new UpdateBranchConfigurationCommand(id,
            updateRequest.MaxTicketsPerDay,
            updateRequest.OpenTime,
            updateRequest.CloseTime,
            updateRequest.BreakStartTime,
            updateRequest.BreakEndTime);

        _mockMediator.Setup(s => s.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        //Act
        var result = await _branchConfigurationController.PutAsync(id, updateRequest);


        //Assert
        result.ShouldNotBeNull();
        var statusCode = result.ShouldBeOfType<OkObjectResult>();
        var returnValue = statusCode.Value.ShouldBeOfType<BranchConfigurationResponseModel>();

        returnValue.Id.ShouldBe(id);
        returnValue.MaxTicketsPerDay.ShouldBe(expectedResponse.MaxTicketsPerDay);
    }
    
    
    [Fact]
    public async Task Should_Return_NotFound_When_BranchConfigurationDoesNotExist()
    {
        int id = 1;
        var updateRequest = new UpdateBranchConfigurationRequest()
        {
            MaxTicketsPerDay = 50,
            OpenTime = new TimeOnly(08,00,00),
            CloseTime = new TimeOnly(18,00,00),
            BreakStartTime = new TimeOnly(13,00,00),
            BreakEndTime = new TimeOnly(14,00,00)
        };

        var updateCommand = new UpdateBranchConfigurationCommand(id,
            updateRequest.MaxTicketsPerDay,
            updateRequest.OpenTime,
            updateRequest.CloseTime,
            updateRequest.BreakStartTime,
            updateRequest.BreakEndTime);
        
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Branch not found");
    
        _mockMediator
            .Setup(s => s.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _branchConfigurationController.PutAsync(id, updateRequest);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Branch not found");

    }
}