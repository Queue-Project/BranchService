using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Branches.Queries.GetBranchById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchControllerTests;

public class GetBranchByIdEndpointTests
{
     private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchController>> _mockLogger;
    private readonly BranchController _branchController;

    public GetBranchByIdEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchController>>();
        _branchController = new BranchController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_BranchById_WithOkStatusCode()
    {
        var branchId = 1;
        var query = new GetBranchByIdQuery(branchId);

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
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _branchController.GetById(branchId);

        // Assert
        result.ShouldNotBeNull();
        
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(200);
        
        var returnedValue = okResult.Value.ShouldBeOfType<BranchResponseModel>();
        returnedValue.Id.ShouldBe(expectedResponse.Id);
        returnedValue.BranchName.ShouldBe(expectedResponse.BranchName);
        returnedValue.Address.ShouldBe(expectedResponse.Address);
        returnedValue.EmailAddress.ShouldBe(expectedResponse.EmailAddress);
        returnedValue.PhoneNumber.ShouldBe(expectedResponse.PhoneNumber);
        

    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_BranchDoesNotExist()
    {
        var branchId = 999;
        var query = new GetBranchByIdQuery(branchId);
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Branch not found");
    
        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _branchController.GetById(branchId);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Branch not found");

    }
}