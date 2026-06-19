using BranchService.Application.UseCases.Companies.Queries.GetAllCompanies;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyTests;

public class GetAllCompaniesQueryHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<GetAllCompaniesQueryHandler>> _mockLogger;
    private readonly GetAllCompaniesQueryHandler _handler;

    public GetAllCompaniesQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<GetAllCompaniesQueryHandler>>();
        _handler = new GetAllCompaniesQueryHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Should_Return_Companies_Successfully()
    {
        
        //Arrange
        var companies = TestDataSeeder.CreateCompanies();
        await _dbContext.Companies.AddRangeAsync(companies, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);


        var query = new GetAllCompaniesQuery(1);
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        result.Should().NotBeNull();
        result.HasNextPage.ShouldBe(false);
        result.TotalPages.ShouldBe(1);
        result.TotalCount.ShouldBe(3);

        var company = result.Items.FirstOrDefault();
        company!.Id.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Return_CompanyEmptyList()
    {
        //Arrange
        var query = new GetAllCompaniesQuery(1);
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        
        result.TotalCount.ShouldBe(0);
        result.HasNextPage.ShouldBe(false);
        var companyList = result.Items;
        companyList.ShouldBeEmpty();
        
    }
}