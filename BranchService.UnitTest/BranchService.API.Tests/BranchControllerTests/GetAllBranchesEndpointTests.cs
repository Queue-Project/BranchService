using BranchService.API.Controllers;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Branches.Queries.GetAllBranches;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.API.Tests.BranchControllerTests;

public class GetAllBranchesEndpointTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<BranchController>> _mockLogger;
    private readonly BranchController _branchController;

    public GetAllBranchesEndpointTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<BranchController>>();
        _branchController = new BranchController(_mockLogger.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task Should_Return_AllBranches_WithOkStatusCode()
    {
        var pageNumber = 1;
        var query = new GetAllBranchesQuery(pageNumber);

        var expectedResponse = new PagedResponse<BranchResponseModel>
        {
            Items =
            [
                new BranchResponseModel()
                {
                    Id = 1,
                    CompanyId = 1,
                    BranchName = "Test Branch",
                    City = "Test City",
                    Address = "Test Street",
                    EmailAddress = "test@branch.com",
                    PhoneNumber = "992923324252",
                    IsActive = true
                },
                new BranchResponseModel()
                {
                    Id = 2,
                    CompanyId = 1,
                    BranchName = "Test Branch2",
                    City = "Test City2",
                    Address = "Test Street2",
                    EmailAddress = "test2@branch.com",
                    PhoneNumber = "992923324252",
                    IsActive = true
                }
            ],
            PageNumber = 1,
            PageSize = 15
        };

        _mockMediator
            .Setup(s => s.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _branchController.GetAll(pageNumber);

        // Assert
        result.ShouldNotBeNull();

        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(200);

        var returnedValue = okResult.Value.ShouldBeOfType<PagedResponse<BranchResponseModel>>();
        returnedValue.Items.Count.ShouldBe(2);
        returnedValue.HasNextPage.ShouldBe(false);
    }
    
}