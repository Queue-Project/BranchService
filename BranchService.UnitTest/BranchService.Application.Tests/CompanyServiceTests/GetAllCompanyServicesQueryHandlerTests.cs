using BranchService.Application.UseCases.Branches.Queries.GetAllBranches;
using BranchService.Application.UseCases.CompanyServices.Queries.GetAllServices;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyServiceTests;

public class GetAllCompanyServicesQueryHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<GetAllServicesQueryHandler>> _mockLogger;
    private readonly GetAllServicesQueryHandler _handler;

    public GetAllCompanyServicesQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<GetAllServicesQueryHandler>>();
        _handler = new GetAllServicesQueryHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Should_Return_CompanyServices_Successfully()
    {
        
        //Arrange
        var companyServices = TestDataSeeder.CreateCompanyServices();
        await _dbContext.CompanyServices.AddRangeAsync(companyServices, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);


        var query = new GetAllServicesQuery(1);
        
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
    public async Task Should_Return_CompanyServicesEmptyList()
    {
        //Arrange
        var query = new GetAllServicesQuery(1);
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        
        result.TotalCount.ShouldBe(0);
        result.HasNextPage.ShouldBe(false);
        var companyList = result.Items;
        companyList.ShouldBeEmpty();
        
    }
}