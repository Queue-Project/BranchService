using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchServiceTest;

public class GetCompanyDetailsTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<global::BranchService.Application.Services.BranchService>> _mockLogger;
    private readonly global::BranchService.Application.Services.BranchService _branchService;

    public GetCompanyDetailsTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<global::BranchService.Application.Services.BranchService>>();
        _branchService = new global::BranchService.Application.Services.BranchService(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task GetCompanyDetails_WhenCompanyIdsExists_ReturnsValidResponse()
    {
        //Arrange
        List<int> ids = [1, 2, 3];

        var companies = TestDataSeeder.CreateCompanies();
        await _dbContext.AddRangeAsync(companies, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        //Act
        var result = await _branchService.GetCompanyDetails(ids);
        
        //Arrange
        
        result.ShouldNotBeEmpty();
        var company = result.FirstOrDefault();
        company!.CompanyId.ShouldBe(companies.First().Id);
     
    }

    [Fact]
    public async Task GetCompanyDetails_WhenIdCountIs0_ReturnsEmptyListResponse()
    {
        //Arrange
        List<int> ids = [];

        var companies = TestDataSeeder.CreateCompanies();
        await _dbContext.AddRangeAsync(companies, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        //Act
        var result = await _branchService.GetCompanyDetails(ids);
        
        //Arrange
        
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }
}