using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Branches.Queries.GetBranchById;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchTests;

public class GetBranchByIdQueryHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<GetBranchByIdQueryHandler>> _mockLogger;
    private readonly GetBranchByIdQueryHandler _handler;

    public GetBranchByIdQueryHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<GetBranchByIdQueryHandler>>();
        _handler = new GetBranchByIdQueryHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Handle_Should_Return_Branch()
    {
        //Arrange
        var branch = TestDataSeeder.CreateBranch();

        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var query = new GetBranchByIdQuery(1);
        
        //Act

        var result = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        result.Should().NotBeNull();
        result.Id.ShouldBe(1);
        
        result.BranchName.ShouldBe(branch.BranchName);
        
    }


    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        var query = new GetBranchByIdQuery(1);
        
        //Act

        var result =  _handler.Handle(query, CancellationToken.None);
        
        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}