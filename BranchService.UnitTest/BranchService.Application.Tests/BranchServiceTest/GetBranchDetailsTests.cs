using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchServiceTest;

public class GetBranchDetailsTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<global::BranchService.Application.Services.BranchService>> _mockLogger;
    private readonly global::BranchService.Application.Services.BranchService _branchService;

    public GetBranchDetailsTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<global::BranchService.Application.Services.BranchService>>();
        _branchService = new global::BranchService.Application.Services.BranchService(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task GetBranchDetails_WhenBRanchIdsExist_ReturnsValidResponse()
    {
        //Arrange
        List<int> ids = [1, 2, 3];

        var company = TestDataSeeder.CreateCompany();
        var branches = TestDataSeeder.CreateBranches();
        await _dbContext.AddAsync(company, CancellationToken.None);
        await _dbContext.AddRangeAsync(branches, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        //Act
        var result = await _branchService.GetBranchDetails(ids);
        
        //Arrange
        
        result.ShouldNotBeEmpty();
        var branch = result.FirstOrDefault();
        branch!.BranchId.ShouldBe(branches.First().Id);
     
    }

    [Fact]
    public async Task GetBranchDetails_WhenIdCountIs0_ReturnsEmptyListResponse()
    {
        //Arrange
        List<int> ids = [];

        var branches = TestDataSeeder.CreateBranches();
        await _dbContext.AddRangeAsync(branches, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        //Act
        var result = await _branchService.GetBranchDetails(ids);
        
        //Arrange
        
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }
}