using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Companies.Queries.GetCompanyById;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyTests;

public class GetCompanyByIdQueryHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<GetCompanyByIdQueryHandler>> _mockLogger;
    private readonly GetCompanyByIdQueryHandler _handler;

    public GetCompanyByIdQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<GetCompanyByIdQueryHandler>>();
        _handler = new GetCompanyByIdQueryHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Handle_Should_Return_Company()
    {
        //Arrange
        var company = TestDataSeeder.CreateCompany();

        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var query = new GetCompanyByIdQuery(1);
        
        //Act

        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        result.Should().NotBeNull();
        result.Id.ShouldBe(1);
        
        result.CompanyName.ShouldBe(company.CompanyName);
        
    }


    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        var query = new GetCompanyByIdQuery(1);
        
        //Act

        var result =  _handler.Handle(query, CancellationToken.None);
        
        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}