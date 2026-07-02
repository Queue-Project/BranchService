using BranchService.API.Controllers;
using BranchService.Application.Response;
using BranchService.Application.UseCases.CompanyServices.Queries.GetAllServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.CompanyServiceControllerTests;

public class GetAllCompanyServicesEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<CompanyServiceController>> _mockLogger;
    private readonly CompanyServiceController _companyServiceController;

    public GetAllCompanyServicesEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<CompanyServiceController>>();
        _companyServiceController = new CompanyServiceController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_AllCompanies_WithOkStatusCode()
    {
        var pageNumber = 1;
        var query = new GetAllServicesQuery(pageNumber);

        var expectedResponse = new PagedResponse<CompanyServiceResponseModel>
        {
            Items =
            [
                new CompanyServiceResponseModel
                {
                    Id = 1,
                    CompanyId = 1,
                    ServiceName = "Test Service Name",
                    ServiceDescription = "Test Service Description"
                },
                new CompanyServiceResponseModel
                {
                    Id = 1,
                    CompanyId = 1,
                    ServiceName = "Test Service Name",
                    ServiceDescription = "Test Service Description"
                }
            ],
            PageNumber = 1,
            PageSize = 15
        };

        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _companyServiceController.GetAllAsync(pageNumber);

        // Assert
        result.ShouldNotBeNull();

        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(200);

        var returnedValue = okResult.Value.ShouldBeOfType<PagedResponse<CompanyServiceResponseModel>>();
        returnedValue.Items.Count.ShouldBe(2);
        returnedValue.HasNextPage.ShouldBe(false);
    }
}