using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Companies.Commands.DeleteCompany;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyControllerTests;

public class DeleteCompanyEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyController>> _mockLogger;
    private readonly CompanyController _companyController;

    public DeleteCompanyEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyController>>();
        _companyController = new CompanyController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_DeletedCompany_WithNoContentStatus()
    {
        //Arrange
        var deleteCommand = new DeleteCompanyCommand(1);

       _mockMediator.Setup(s => s.Send(deleteCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
       
       
       //Act
       var result = await _companyController.DeleteAsync(deleteCommand.Id);
       
       //Assert
       result.ShouldBeOfType<NoContentResult>();

    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_CompanyDoesNotExist()
    {
        int id = 999;
        var deleteCommand = new DeleteCompanyCommand(id);
        
        
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Company not found");
    
        _mockMediator
            .Setup(s => s.Send(deleteCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _companyController.DeleteAsync(id);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Company not found");

    }
}