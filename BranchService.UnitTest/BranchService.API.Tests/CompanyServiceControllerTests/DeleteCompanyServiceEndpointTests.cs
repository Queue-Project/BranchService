using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.CompanyServices.Commands.DeleteService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyServiceControllerTests;

public class DeleteCompanyServiceEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyServiceController>> _mockLogger;
    private readonly CompanyServiceController _companyServiceController;

    public DeleteCompanyServiceEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyServiceController>>();
        _companyServiceController = new CompanyServiceController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_DeletedCompany_WithNoContentStatus()
    {
        //Arrange
        var deleteCommand = new DeleteServiceCommand(1);

       _mockMediator.Setup(s => s.Send(deleteCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
       
       
       //Act
       var result = await _companyServiceController.Delete(deleteCommand.Id);
       
       //Assert
       result.ShouldBeOfType<NoContentResult>();

    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_CompanyDoesNotExist()
    {
        int id = 999;
        var deleteCommand = new DeleteServiceCommand(id);
        
        
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Company not found");
    
        _mockMediator
            .Setup(s => s.Send(deleteCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _companyServiceController.Delete(id);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Company not found");

    }
}