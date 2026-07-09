using BranchService.API.Controllers;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Companies.Queries.GetAllCompanies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyControllerTests;

public class GetAllCompaniesEndpointTests
{
     private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyController>> _mockLogger;
    private readonly CompanyController _companyController;

    public GetAllCompaniesEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyController>>();
        _companyController = new CompanyController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_AllCompanies_WithOkStatusCode()
    {
        var pageNumber = 1;
        var query = new GetAllCompaniesQuery(pageNumber);

        var expectedResponse = new PagedResponse<CompanyResponseModel>
        {
            Items = [
            new CompanyResponseModel
            {
                Id = 1,
                CompanyName = "Test Company",
                Address = "Test Street",
                EmailAddress = "test@company.com",
                PhoneNumber = "992923324252"
            },
            new CompanyResponseModel
            {
                Id = 2,
                CompanyName = "Test Company2",
                Address = "Test Street2",
                EmailAddress = "test2@company.com",
                PhoneNumber = "992923324222"
            }
            ],
            PageNumber = 1,
            PageSize = 15
        };

        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _companyController.GetAllAsync(pageNumber);

        // Assert
        result.ShouldNotBeNull();
        
        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(200);
        
        var returnedValue = okResult.Value.ShouldBeOfType<PagedResponse<CompanyResponseModel>>();
        returnedValue.Items.Count.ShouldBe(2);
        returnedValue.HasNextPage.ShouldBe(false);
        

    }
    
}