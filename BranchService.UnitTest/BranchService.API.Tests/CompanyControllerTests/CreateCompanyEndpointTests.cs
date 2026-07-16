using BranchService.API.Controllers;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Companies.Commands.CreateCompany;
using BranchService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyControllerTests;

public class CreateCompanyEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyController>> _mockLogger;
    private readonly CompanyController _companyController;

    public CreateCompanyEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyController>>();
        _companyController = new CompanyController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_CreatedCompany_WithOkStatusCode()
    {
        // Arrange
        var createCompanyCommand = new CreateCompanyCommand
        (
            "Test Company",
            "123 Test Street",
            "test@company.com",
            "+992924567686",
            CompanyCategory.Beauty
        );

        var expectedResponse = new CompanyResponseModel
        {
            Id = 1,
            CompanyName = "Test Company",
            Address = "Test Street",
            EmailAddress = "test@company.com",
            PhoneNumber = "992923324252",
            CompanyCategory = CompanyCategory.Beauty
        };

        _mockMediator
            .Setup(m => m.Send(createCompanyCommand, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _companyController.PostAsync(createCompanyCommand);

        // Assert
        var okResult = result.ShouldBeOfType<OkObjectResult>();
        var returnValue = okResult.Value.ShouldBeOfType<CompanyResponseModel>();

        returnValue.Id.ShouldBe(expectedResponse.Id);
        returnValue.CompanyName.ShouldBe(expectedResponse.CompanyName);
    }

    [Fact]
    public async Task Should_Return_BadRequest_When_InvalidCommand()
    {
        // Arrange
        var createCompanyCommand = new CreateCompanyCommand(
            CompanyName: "",
            Address: "123 Test Street",
            EmailAddress: "invalid-email",
            PhoneNumber: "123",
            CompanyCategory.Beauty
            
        );

        _mockMediator
            .Setup(m => m.Send(createCompanyCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Validation failed"));


        //Act
        var result = _companyController.PostAsync(createCompanyCommand);

        //Assert
        var exception = result.ShouldThrow<FluentValidation.ValidationException>();
        exception.Message.ShouldBe("Validation failed");
    }
}