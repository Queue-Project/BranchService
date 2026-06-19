using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Branches.Commands.DeleteBranch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchControllerTests;

public class DeleteBranchEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchController>> _mockLogger;
    private readonly BranchController _branchController;

    public DeleteBranchEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchController>>();
        _branchController = new BranchController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_DeletedBranch_WithNoContentStatus()
    {
        //Arrange
        var deleteCommand = new DeleteBranchCommand(1);

       _mockMediator.Setup(s => s.Send(deleteCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
       
       
       //Act
       var result = await _branchController.DeleteAsync(deleteCommand.Id);
       
       //Assert
       result.ShouldBeOfType<NoContentResult>();

    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_BranchDoesNotExist()
    {
        int id = 999;
        var deleteCommand = new DeleteBranchCommand(id);
        
        
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Branch not found");
    
        _mockMediator
            .Setup(s => s.Send(deleteCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _branchController.DeleteAsync(id);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Branch not found");

    }
}