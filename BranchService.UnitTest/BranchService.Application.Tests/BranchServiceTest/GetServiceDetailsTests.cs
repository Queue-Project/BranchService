using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchServiceTest;

public class GetServiceDetailsTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<global::BranchService.Application.Services.BranchService>> _mockLogger;
    private readonly global::BranchService.Application.Services.BranchService _branchService;

    public GetServiceDetailsTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<global::BranchService.Application.Services.BranchService>>();
        _branchService = new global::BranchService.Application.Services.BranchService(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task GetServiceDetails_WhenServiceIdsExist_ReturnsValidResponse()
    {
        //Arrange
        List<int> ids = [1, 2, 3];

        var company = TestDataSeeder.CreateCompany();
        var branch = TestDataSeeder.CreateBranch();
        var services = TestDataSeeder.CreateCompanyServices();
        await _dbContext.AddAsync(company, CancellationToken.None);
        await _dbContext.AddAsync(branch, CancellationToken.None);
        await _dbContext.AddRangeAsync(services, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        //Act
        var result = await _branchService.GetServiceDetails(ids);
        
        //Arrange
        
        result.ShouldNotBeEmpty();
        var service = result.FirstOrDefault();
        service!.ServiceId.ShouldBe(services.First().Id);
     
    }

    [Fact]
    public async Task GetServiceDetails_WhenIdCountIs0_ReturnsEmptyListResponse()
    {
        //Arrange
        List<int> ids = [];

        
        
        //Act
        var result = await _branchService.GetBranchDetails(ids);
        
        //Arrange
        
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }
}