using System.Net;
using BranchService.API.Controllers;
using BranchService.Application.Exceptions;
using BranchService.Application.Requests;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Companies.Commands.UpdateCompany;
using BranchService.Application.UseCases.Companies.Queries.GetCompanyById;
using BranchService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyControllerTests;

public class UpdateCompanyEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyController>> _mockLogger;
    private readonly CompanyController _companyController;

    public UpdateCompanyEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyController>>();
        _companyController = new CompanyController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_UpdatedCompany_WithOkStatusCode()
    {
        //Arrange
        int id = 1;
        var updateRequest = new UpdateCompanyRequest
        {
            CompanyName = "Update Company Name",
            Address = "Update Address",
            EmailAddress = "update@test.com",
            PhoneNumber = "+992923224252",
            CompanyCategory = CompanyCategory.Beauty
            
        };


        var expectedResponse = new CompanyResponseModel
        {
            Id = 1,
            CompanyName = "Update Company Name",
            Address = "Update Address",
            EmailAddress = "update@test.com",
            PhoneNumber = "+992923224252",
        };

        var updateCommand = new UpdateCompanyCommand(id,
            updateRequest.CompanyName,
            updateRequest.Address,
            updateRequest.EmailAddress,
            updateRequest.PhoneNumber,
            updateRequest.CompanyCategory);

        _mockMediator.Setup(s => s.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        //Act
        var result = await _companyController.PutAsync(id, updateRequest);


        //Assert
        result.ShouldNotBeNull();
        var statusCode = result.ShouldBeOfType<OkObjectResult>();
        var returnValue = statusCode.Value.ShouldBeOfType<CompanyResponseModel>();

        returnValue.Id.ShouldBe(id);
        returnValue.CompanyName.ShouldBe(expectedResponse.CompanyName);
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_InvalidCommand()
    {
        // Arrange

        int id = 1;
        var updateRequest = new UpdateCompanyRequest
        {
            CompanyName = "",
            Address = "Update Address",
            EmailAddress = "updateTest.com",
            PhoneNumber = "A11",
            CompanyCategory = CompanyCategory.Beauty
            
        };

        var updateCommand = new UpdateCompanyCommand(id,
            updateRequest.CompanyName,
            updateRequest.Address,
            updateRequest.EmailAddress,
            updateRequest.PhoneNumber,
            updateRequest.CompanyCategory);

        _mockMediator
            .Setup(m => m.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));


        //Act
        var result = _companyController.PutAsync(id, updateRequest);

        //Assert
        var exception = result.ShouldThrow<FluentValidation.ValidationException>();
        exception.Message.ShouldBe("Validation failed");
    }
    
    [Fact]
    public async Task Should_Return_NotFound_When_CompanyDoesNotExist()
    {
        int id = 999;
        var updateRequest = new UpdateCompanyRequest
        {
            CompanyName = "",
            Address = "Update Address",
            EmailAddress = "updateTest.com",
            PhoneNumber = "A11",
            CompanyCategory = CompanyCategory.Beauty
        };

        var updateCommand = new UpdateCompanyCommand(id,
            updateRequest.CompanyName,
            updateRequest.Address,
            updateRequest.EmailAddress,
            updateRequest.PhoneNumber,
            updateRequest.CompanyCategory);
        
        var expectedException = new HttpStatusCodeException(HttpStatusCode.NotFound, "Company not found");
    
        _mockMediator
            .Setup(s => s.Send(updateCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var result =  _companyController.PutAsync(id, updateRequest);

        //Assert
        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe("Company not found");

    }
}