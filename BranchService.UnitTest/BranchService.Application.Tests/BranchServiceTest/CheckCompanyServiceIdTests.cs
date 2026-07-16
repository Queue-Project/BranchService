using BranchService.Contracts.Requests;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchServiceTest;

public class CheckCompanyServiceIdTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<global::BranchService.Application.Services.BranchService>> _mockLogger;
    private readonly global::BranchService.Application.Services.BranchService _branchService;

    public CheckCompanyServiceIdTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<global::BranchService.Application.Services.BranchService>>();
        _branchService = new global::BranchService.Application.Services.BranchService(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task CheckCompanyServiceId_WhenCompanyServiceDoesNotExist_ReturnsInvalidResponse()
    {
        //Arrange
        
        var request = new CompanyServiceRequest
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            BranchId = 1,
            CompanyServiceId = 1,
            RequestedAt = DateTimeOffset.UtcNow
        };
        
        
        //Act
        var result = await _branchService.CheckCompanyServiceId(request);
        
        //Arrange
        
        result.IsValid.ShouldBe(false);
        result.ErrorMessage.ShouldNotBeNull();
    }

    [Fact]
    public async Task CheckCompanyId_WhenCompanyServiceExists_ReturnsValidResponse()
    {
        //Arrange
        
        var company = TestDataSeeder.CreateCompany();
        var companyService = TestDataSeeder.CreateCompanyService();
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.CompanyServices.AddAsync(companyService, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var request = new CompanyServiceRequest
        {
            RequestId = Guid.NewGuid(),
            CompanyId = 1,
            BranchId = 1,
            CompanyServiceId = 1,
            RequestedAt = DateTimeOffset.UtcNow
        };
        
        //Act
        var result = await _branchService.CheckCompanyServiceId(request);
        
        //Arrange
        
        result.RequestId.ShouldBe(result.RequestId);
        result.CompanyId.ShouldBe(result.CompanyId);
        result.CompanyServiceId.ShouldBe(result.CompanyServiceId);
        result.IsValid.ShouldBe(true);
        result.ErrorMessage.ShouldBeNull();
    }

}