using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.BranchConfigurations.Commands.DeleteBranchConfiguration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchConfigurationControllerTests;

public class DeleteBranchConfigurationEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchConfigurationController>> _mockLogger;
    private readonly BranchConfigurationController _branchConfigurationController;

    public DeleteBranchConfigurationEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchConfigurationController>>();
        _branchConfigurationController = new BranchConfigurationController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_DeletedBranchConfiguration_WithNoContentStatus()
    {
        //Arrange
        var deleteCommand = new DeleteBranchConfigurationCommand(1);

        _mockMediator.Setup(s => s.Send(deleteCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
       
       
        //Act
        var result = await _branchConfigurationController.DeleteAsync(deleteCommand.Id);
       
        //Assert
        result.ShouldBeOfType<NoContentResult>();

    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_BranchConfigurationDoesNotExist()
    {
        int id = 999;
        var deleteCommand = new DeleteBranchConfigurationCommand(id);
        
        
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Branch Configuration not found");
    
        _mockMediator
            .Setup(s => s.Send(deleteCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _branchConfigurationController.DeleteAsync(id);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Branch Configuration not found");

    }
}