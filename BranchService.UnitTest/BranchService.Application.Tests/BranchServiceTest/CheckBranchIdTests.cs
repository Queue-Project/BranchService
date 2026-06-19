using BranchService.Contracts.Requests;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchServiceTest;

public class CheckBranchIdTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<global::BranchService.Application.Services.BranchService>> _mockLogger;
    private readonly global::BranchService.Application.Services.BranchService _branchService;

    public CheckBranchIdTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<global::BranchService.Application.Services.BranchService>>();
        _branchService = new global::BranchService.Application.Services.BranchService(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task CheckBranchId_WhenBranchDoesNotExist_ReturnsInvalidResponse()
    {
        //Arrange
        
        var request = new BranchRequest()
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            BranchId = 1,
            RequestedAt = DateTimeOffset.UtcNow
        };
        
        
        //Act
        var result = await _branchService.CheckBranchId(request);
        
        //Arrange
        
        result.IsValid.ShouldBe(false);
        result.ErrorMessage.ShouldNotBeNull();
    }
    

    [Fact]
    public async Task CheckBranchId_WhenBranchExists_ReturnsValidResponse()
    {
        //Arrange
        
        var company = TestDataSeeder.CreateCompany();
        var branch = TestDataSeeder.CreateBranch();
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var request = new BranchRequest()
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            BranchId = 1,
            RequestedAt = DateTimeOffset.UtcNow
        };
        
        
        //Act
        var result = await _branchService.CheckBranchId(request);
        
        //Arrange
        
        result.RequestId.ShouldBe(result.RequestId);
        result.CompanyId.ShouldBe(result.CompanyId);
        result.BranchId.ShouldBe(result.BranchId);
        result.IsValid.ShouldBe(true);
        result.ErrorMessage.ShouldBeNull();
    }

}