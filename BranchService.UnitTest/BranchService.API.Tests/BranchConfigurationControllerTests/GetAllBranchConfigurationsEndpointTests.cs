using BranchService.API.Controllers;
using BranchService.Application.Response;
using BranchService.Application.UseCases.BranchConfigurations.Queries.GetAllBranchConfigurations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchConfigurationControllerTests;

public class GetAllBranchConfigurationsEndpointTests
{
     private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchConfigurationController>> _mockLogger;
    private readonly BranchConfigurationController _branchConfigurationController;

    public GetAllBranchConfigurationsEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchConfigurationController>>();
        _branchConfigurationController = new BranchConfigurationController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_AllBranchConfigurations_WithOkStatusCode()
    {
        var pageNumber = 1;
        var query = new GetAllBranchConfigurationsQuery(pageNumber);

        var expectedResponse = new PagedResponse<BranchConfigurationResponseModel>
        {
            Items =
            [
               new BranchConfigurationResponseModel
               {
                   Id = 1,
                   BranchId = 1,
                   MaxTicketsPerDay = 50,
                   OpenTime = new TimeOnly(08, 00, 00),
                   CloseTime =   new TimeOnly(18, 00, 00), 
                   BreakStartTime = new TimeOnly(13, 00, 00),
                   BreakEndTime = new TimeOnly(14, 00, 00)
               },
               new BranchConfigurationResponseModel
               {
                   Id = 2,
                   BranchId = 2,
                   MaxTicketsPerDay = 50,
                   OpenTime = new TimeOnly(08, 00, 00),
                   CloseTime =   new TimeOnly(18, 00, 00), 
                   BreakStartTime = new TimeOnly(13, 00, 00),
                   BreakEndTime = new TimeOnly(14, 00, 00)
               }
            ],
            PageNumber = 1,
            PageSize = 15
        };

        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _branchConfigurationController.GetAll(pageNumber);

        // Assert
        result.ShouldNotBeNull();

        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(200);

        var returnedValue = okResult.Value.ShouldBeOfType<PagedResponse<BranchConfigurationResponseModel>>();
        returnedValue.Items.Count.ShouldBe(2);
        returnedValue.HasNextPage.ShouldBe(false);
    }
}