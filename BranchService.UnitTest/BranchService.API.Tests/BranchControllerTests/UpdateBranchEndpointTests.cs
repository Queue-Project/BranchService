using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Requests;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Branches.Commands.UpdateBranch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchControllerTests;

public class UpdateBranchEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchController>> _mockLogger;
    private readonly BranchController _branchController;

    public UpdateBranchEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchController>>();
        _branchController = new BranchController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_UpdatedBranch_WithOkStatusCode()
    {
        //Arrange
        int id = 1;
        var updateRequest = new UpdateBranchRequest()
        {
            BranchName = "Update Branch Name",
            Address = "Update Address",
            EmailAddress = "update@test.com",
            PhoneNumber = "+992923224252",
            City = "Update City",
            CompanyId = 1
        };


        var expectedResponse = new BranchResponseModel()
        {
            Id = 1,
            CompanyId = 1,
            BranchName = "Update Branch Name",
            City = "Update City",
            Address = "Update Address",
            EmailAddress = "update@test.com",
            PhoneNumber = "+992923224252",
            IsActive = true
        };

        var updateCommand = new UpdateBranchCommand(id,
            updateRequest.BranchName,
            updateRequest.City,
            updateRequest.Address,
            updateRequest.PhoneNumber,
            updateRequest.EmailAddress);

        _mockMediator.Setup(s => s.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        //Act
        var result = await _branchController.PutAsync(id, updateRequest);


        //Assert
        result.ShouldNotBeNull();
        var statusCode = result.ShouldBeOfType<OkObjectResult>();
        var returnValue = statusCode.Value.ShouldBeOfType<BranchResponseModel>();

        returnValue.Id.ShouldBe(id);
        returnValue.BranchName.ShouldBe(expectedResponse.BranchName);
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_InvalidCommand()
    {
        // Arrange

        int id = 1;
        var updateRequest = new UpdateBranchRequest()
        {
            BranchName = "Update Branch Name",
            Address = "Update Address",
            EmailAddress = "update@test.com",
            PhoneNumber = "+992923224252",
            City = "Update City",
            CompanyId = 1
        };

        var updateCommand = new UpdateBranchCommand(id,
            updateRequest.BranchName,
            updateRequest.City,
            updateRequest.Address,
            updateRequest.PhoneNumber,
            updateRequest.EmailAddress);

        _mockMediator
            .Setup(m => m.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));


        //Act
        var result = _branchController.PutAsync(id, updateRequest);

        //Assert
        var exception = result.ShouldThrow<FluentValidation.ValidationException>();
        exception.Message.ShouldBe("Validation failed");
    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_BranchDoesNotExist()
    {
        int id = 999;
        var updateRequest = new UpdateBranchRequest()
        {
            BranchName = "Update Branch Name",
            Address = "Update Address",
            EmailAddress = "update@test.com",
            PhoneNumber = "+992923224252",
            City = "Update City",
            CompanyId = 1
        };

        var updateCommand = new UpdateBranchCommand(id,
            updateRequest.BranchName,
            updateRequest.City,
            updateRequest.Address,
            updateRequest.PhoneNumber,
            updateRequest.EmailAddress);
        
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Company not found");
    
        _mockMediator
            .Setup(s => s.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _branchController.PutAsync(id, updateRequest);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Company not found");

    }
}