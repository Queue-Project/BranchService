using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Response;
using BranchService.Application.UseCases.BranchConfigurations.Queries.GetBranchConfigurationById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchConfigurationControllerTests;

public class GetBranchConfigurationByIdEndpointTests
{
     private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchConfigurationController>> _mockLogger;
    private readonly BranchConfigurationController _branchConfigurationController;

    public GetBranchConfigurationByIdEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchConfigurationController>>();
        _branchConfigurationController = new BranchConfigurationController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_BranchConfigurationById_WithOkStatusCode()
    {
        var branchConfigurationId = 1;
        var query = new GetBranchConfigurationByIdQuery(branchConfigurationId);

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

        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _branchConfigurationController.GetById(branchConfigurationId);

        // Assert
        result.ShouldNotBeNull();
        
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(200);
        
        var returnedValue = okResult.Value.ShouldBeOfType<BranchConfigurationResponseModel>();
        returnedValue.Id.ShouldBe(expectedResponse.Id);
        returnedValue.MaxTicketsPerDay.ShouldBe(expectedResponse.MaxTicketsPerDay);
        
    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_BranchConfigurationDoesNotExist()
    {
        var branchId = 999;
        var query = new GetBranchConfigurationByIdQuery(branchId);
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Branch Configuration not found");
    
        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _branchConfigurationController.GetById(branchId);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Branch Configuration not found");

    }
}