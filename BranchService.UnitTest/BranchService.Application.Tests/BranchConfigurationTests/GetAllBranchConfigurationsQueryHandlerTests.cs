using BranchService.Application.UseCases.BranchConfigurations.Queries.GetAllBranchConfigurations;
using BranchService.Application.UseCases.Branches.Queries.GetAllBranches;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchConfigurationTests;

public class GetAllBranchConfigurationsQueryHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<GetAllBranchConfigurationsQueryHandler>> _mockLogger;
    private readonly GetAllBranchConfigurationsQueryHandler _handler;

    public GetAllBranchConfigurationsQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<GetAllBranchConfigurationsQueryHandler>>();
        _handler = new GetAllBranchConfigurationsQueryHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Should_Return_Companies_Successfully()
    {
        
        //Arrange
        var branchConfigurations = TestDataSeeder.CreateBranchConfigurations();
        await _dbContext.BranchConfigurations.AddRangeAsync(branchConfigurations, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);


        var query = new GetAllBranchConfigurationsQuery(1);
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        result.Should().NotBeNull();
        result.HasNextPage.ShouldBe(false);
        result.TotalPages.ShouldBe(1);
        result.TotalCount.ShouldBe(3);

        var branch = result.Items.FirstOrDefault();
        branch!.Id.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Return_CompanyEmptyList()
    {
        //Arrange
        var query = new GetAllBranchConfigurationsQuery(1);
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        
        result.TotalCount.ShouldBe(0);
        result.HasNextPage.ShouldBe(false);
        var companyList = result.Items;
        companyList.ShouldBeEmpty();
        
    }
}