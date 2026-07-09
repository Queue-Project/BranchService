using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Companies.Queries.GetCompanyById;
using BranchService.Application.UseCases.Companies.Queries.GetCompanyInfoById;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyTests;

public class GetCompanyInfoByIdQueryHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<GetCompanyInfoByIdQueryHandler>> _mockLogger;
    private readonly GetCompanyInfoByIdQueryHandler _handler;

    public GetCompanyInfoByIdQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<GetCompanyInfoByIdQueryHandler>>();
        _handler = new GetCompanyInfoByIdQueryHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Should_Return_CompanyInfo_Successfully()
    {
        //Arrange
        var company = TestDataSeeder.CreateCompany();
        var branch = TestDataSeeder.CreateBranch();
        var branchConfiguration = TestDataSeeder.CreatBranchConfiguration();
        var companyService = TestDataSeeder.CreateCompanyService();
        
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        await _dbContext.BranchConfigurations.AddAsync(branchConfiguration, CancellationToken.None);
        await _dbContext.AddAsync(companyService, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);


        var query = new GetCompanyInfoByIdQuery(1);
        
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert

        result.Should().NotBeNull();
        result.TotalBranches.ShouldBe(1);
        result.TotalServices.ShouldBe(1);
        result.Id.ShouldBe(1);
        var branches = result.Branches.FirstOrDefault();
        branches.CompanyId.ShouldBe(1);
        branches.BranchName.ShouldBe("Test Branch");

    }

    [Fact]
    public async Task Should_Return_CompanyInfo_WithoutBranchInfo()
    {
        //Arrange
        var company = TestDataSeeder.CreateCompany();
        var companyService = TestDataSeeder.CreateCompanyService();
        
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.AddAsync(companyService, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
       
        var query = new GetCompanyInfoByIdQuery(1);


        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        //Assert

        result.Should().NotBeNull();
        result.TotalBranches.ShouldBe(0);
        result.TotalServices.ShouldBe(1);
        var branches = result.Branches;
        branches.ShouldBeEmpty();
    }


    [Fact]
    public async Task Should_Return_CompanyInfo_Without_CompanyServiceInfo()
    {
        //Arrange
        var company = TestDataSeeder.CreateCompany();
        var branch = TestDataSeeder.CreateBranch();
        var branchConfiguration = TestDataSeeder.CreatBranchConfiguration();
        
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        await _dbContext.BranchConfigurations.AddAsync(branchConfiguration, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);


        var query = new GetCompanyInfoByIdQuery(1);
        
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);
        //Assert

        result.Should().NotBeNull();
        result.TotalBranches.ShouldBe(1);
        result.TotalServices.ShouldBe(0);
        var branches = result.Branches;
        branches.ShouldNotBeEmpty();
        var services = result.Services;
        services.ShouldBeEmpty();

    }

    [Fact]
    public async Task Should_Return_NotFound()
    {
        //Arrange
        var query = new GetCompanyInfoByIdQuery(1);
        
        //Act

        var result =  _handler.Handle(query, CancellationToken.None);
        
        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}