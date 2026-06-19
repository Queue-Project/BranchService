using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Branches.Commands.CreateBranch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchControllerTests;

public class CreateBranchEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchController>> _mockLogger;
    private readonly BranchController _branchController;

    public CreateBranchEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchController>>();
        _branchController = new BranchController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_CreatedBranch_WithOkStatusCode()
    {
        // Arrange
        var createBranchCommand = new CreateBranchCommand
        (
            "Test Branch",
            "Test City",
            "Test Street",
            "+992923324252",
            "test@brnach.com",
            1
        );

        var expectedResponse = new BranchResponseModel()
        {
            Id = 1,
            CompanyId = 1,
            BranchName = "Test Branch",
            City = "Test City",
            Address = "Test Street",
            EmailAddress = "test@branch.com",
            PhoneNumber = "992923324252",
            IsActive = true
        };

        _mockMediator
            .Setup(m => m.Send(createBranchCommand,It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _branchController.PostAsync(createBranchCommand);

        // Assert
        var okResult = result.ShouldBeOfType<OkObjectResult>();
        var returnValue= okResult.Value.ShouldBeOfType<BranchResponseModel>();   
        
        returnValue.Id.ShouldBe(expectedResponse.Id);
        returnValue.CompanyId.ShouldBe(expectedResponse.CompanyId);
        returnValue.BranchName.ShouldBe(expectedResponse.BranchName);
        
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_InvalidCommand()
    {
        // Arrange
        var createBranchCommand = new CreateBranchCommand
        (
            "",
            "Test City",
            "Test Street",
            "12",
            "testBranch.com",
            1
        );

        _mockMediator
            .Setup(m => m.Send(createBranchCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));
        
        
        //Act
        var result =  _branchController.PostAsync(createBranchCommand);

        //Assert
        var exception=  result.ShouldThrow<FluentValidation.ValidationException>();
        exception.Message.ShouldBe("Validation failed");
        
    }

    [Fact]
    public async Task Should_Return_CompanyNotFound()
    {
        //Arrange
        var createBranchCommand = new CreateBranchCommand
        (
            "Test Branch",
            "Test City",
            "Test Street",
            "+992923324252",
            "test@brnach.com",
            1
        );

        var expectedResponse = new HttpStatusCodeException(HttpStatusCode.NotFound, "Company not found");

        _mockMediator.Setup(s => s.Send(createBranchCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedResponse);
        
        //Act
        var result = _branchController.PostAsync(createBranchCommand);
        
        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Company not found");

    }
    
}