using BranchService.Application.UseCases.Branches.Queries.GetAllBranches;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchTests;

public class GetAllBranchesQueryHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<GetAllBranchesQueryHandler>> _mockLogger;
    private readonly GetAllBranchesQueryHandler _handler;

    public GetAllBranchesQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<GetAllBranchesQueryHandler>>();
        _handler = new GetAllBranchesQueryHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Should_Return_Companies_Successfully()
    {
        
        //Arrange
        var branches = TestDataSeeder.CreateBranches();
        await _dbContext.Branches.AddRangeAsync(branches, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);


        var query = new GetAllBranchesQuery(1);
        
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
        var query = new GetAllBranchesQuery(1);
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        
        result.TotalCount.ShouldBe(0);
        result.HasNextPage.ShouldBe(false);
        var companyList = result.Items;
        companyList.ShouldBeEmpty();
        
    }
}