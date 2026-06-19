using BranchService.Contracts.Requests;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchServiceTest;

public class GetCompanyBranchesTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<global::BranchService.Application.Services.BranchService>> _mockLogger;
    private readonly global::BranchService.Application.Services.BranchService _branchService;

    public GetCompanyBranchesTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<global::BranchService.Application.Services.BranchService>>();
        _branchService = new global::BranchService.Application.Services.BranchService(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task GetBranches_WhenBranchDoesNotExist_ReturnsInvalidResponse()
    {
        //Arrange

        var companyId = 1;
        
        //Act
        var result = await _branchService.GetCompanyBranches(companyId);
        
        //Arrange
        
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetBranches_WhenBranchExists_ReturnsValidResponse()
    {
        //Arrange
        
        var company = TestDataSeeder.CreateCompany();
        var branches = TestDataSeeder.CreateBranches();
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.Branches.AddRangeAsync(branches, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        //Act
        var result = await _branchService.GetCompanyBranches(company.Id);
        
        //Arrange
        
        result.ShouldNotBeEmpty();
        var branch = result.FirstOrDefault();
        branch!.CompanyId.ShouldBe(company.Id);
        branch.IsValid.ShouldBe(true);
        branch.ErrorMessage.ShouldBeNull();
    }

}