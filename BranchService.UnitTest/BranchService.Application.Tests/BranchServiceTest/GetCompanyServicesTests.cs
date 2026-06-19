using BranchService.Contracts.Requests;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchServiceTest;

public class GetCompanyServicesTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<global::BranchService.Application.Services.BranchService>> _mockLogger;
    private readonly global::BranchService.Application.Services.BranchService _branchService;

    public GetCompanyServicesTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<global::BranchService.Application.Services.BranchService>>();
        _branchService = new global::BranchService.Application.Services.BranchService(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task GetServices_WhenServiceDoesNotExist_ReturnsInvalidResponse()
    {
        //Arrange

        var companyId = 1;
        
        //Act
        var result = await _branchService.GetCompanyServices(companyId);
        
        //Arrange
        
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetServices_WhenServiceExists_ReturnsValidResponse()
    {
        //Arrange
        
        var company = TestDataSeeder.CreateCompany();
        var companyServices = TestDataSeeder.CreateCompanyServices();
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.CompanyServices.AddRangeAsync(companyServices, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        //Act
        var result = await _branchService.GetCompanyServices(company.Id);
        
        //Arrange
        
        result.ShouldNotBeEmpty();
        var service = result.FirstOrDefault();
        service!.CompanyId.ShouldBe(company.Id);
        service.IsValid.ShouldBe(true);
        service.ErrorMessage.ShouldBeNull();
    }

}