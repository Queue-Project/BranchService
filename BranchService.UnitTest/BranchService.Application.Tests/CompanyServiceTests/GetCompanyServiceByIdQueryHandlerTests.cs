using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.CompanyServices.Queries.GetServiceById;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyServiceTests;

public class GetCompanyServiceByIdQueryHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<GetServiceByIdQueryHandler>> _mockLogger;
    private readonly GetServiceByIdQueryHandler _handler;

    public GetCompanyServiceByIdQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<GetServiceByIdQueryHandler>>();
        _handler = new GetServiceByIdQueryHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Handle_Should_Return_CompanyService()
    {
        //Arrange
        var companyService = TestDataSeeder.CreateCompanyService();

        await _dbContext.CompanyServices.AddAsync(companyService, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var query = new GetServiceByIdQuery(1);
        
        //Act

        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        result.Should().NotBeNull();
        result.Id.ShouldBe(1);
        
        result.ServiceName.ShouldBe(companyService.ServiceName);
        
    }


    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        var query = new GetServiceByIdQuery(1);
        
        //Act

        var result =  _handler.Handle(query, CancellationToken.None);
        
        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}