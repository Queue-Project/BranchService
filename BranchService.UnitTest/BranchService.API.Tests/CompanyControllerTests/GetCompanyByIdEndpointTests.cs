using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Companies.Queries.GetCompanyById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyControllerTests;

public class GetCompanyByIdEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyController>> _mockLogger;
    private readonly CompanyController _companyController;

    public GetCompanyByIdEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyController>>();
        _companyController = new CompanyController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_CompanyById_WithOkStatusCode()
    {
        var companyId = 1;
        var query = new GetCompanyByIdQuery(companyId);

        var expectedResponse = new CompanyResponseModel
        {
            Id = companyId,
            CompanyName = "Test Company",
            Address = "Test Street",
            EmailAddress = "test@company.com",
            PhoneNumber = "992923324252"
        };

        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _companyController.GetByIdAsync(companyId);

        // Assert
        result.ShouldNotBeNull();
        
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(200);
        
        var returnedValue = okResult.Value.ShouldBeOfType<CompanyResponseModel>();
        returnedValue.Id.ShouldBe(expectedResponse.Id);
        returnedValue.CompanyName.ShouldBe(expectedResponse.CompanyName);
        returnedValue.Address.ShouldBe(expectedResponse.Address);
        returnedValue.EmailAddress.ShouldBe(expectedResponse.EmailAddress);
        returnedValue.PhoneNumber.ShouldBe(expectedResponse.PhoneNumber);
        

    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_CompanyDoesNotExist()
    {
        var companyId = 999;
        var query = new GetCompanyByIdQuery(companyId);
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Company not found");
    
        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _companyController.GetByIdAsync(companyId);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Company not found");

    }
}