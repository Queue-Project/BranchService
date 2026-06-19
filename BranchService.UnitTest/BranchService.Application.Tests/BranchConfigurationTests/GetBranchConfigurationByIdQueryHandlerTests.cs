using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.BranchConfigurations.Queries.GetBranchConfigurationById;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchConfigurationTests;

public class GetBranchConfigurationByIdQueryHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<GetBranchConfigurationByIdQueryHandler>> _mockLogger;
    private readonly GetBranchConfigurationByIdQueryHandler _handler;

    public GetBranchConfigurationByIdQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<GetBranchConfigurationByIdQueryHandler>>();
        _handler = new GetBranchConfigurationByIdQueryHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Handle_Should_Return_Branch()
    {
        //Arrange
        var branchConfiguration = TestDataSeeder.CreatBranchConfiguration();

        await _dbContext.BranchConfigurations.AddAsync(branchConfiguration, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var query = new GetBranchConfigurationByIdQuery(1);
        
        //Act

        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        result.Should().NotBeNull();
        result.Id.ShouldBe(1);
        
        result.MaxTicketsPerDay.ShouldBe(branchConfiguration.MaxTicketsPerDay);
        
    }


    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        var query = new GetBranchConfigurationByIdQuery(1);
        
        //Act

        var result =  _handler.Handle(query, CancellationToken.None);
        
        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}