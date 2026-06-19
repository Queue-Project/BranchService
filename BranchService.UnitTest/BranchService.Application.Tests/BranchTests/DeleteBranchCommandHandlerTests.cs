using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Branches.Commands.DeleteBranch;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchTests;

public class DeleteBranchCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<DeleteBranchCommandHandler>> _mockLogger;
    private readonly DeleteBranchCommandHandler _handler;

    public DeleteBranchCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<DeleteBranchCommandHandler>>();
        _handler = new DeleteBranchCommandHandler(_mockLogger.Object, _dbContext, _mockPublishEndpoint.Object);
    }


    [Fact]
    public async Task Handle_Should_Delete_Branch()
    {
        
        //Arrange
        var branch = TestDataSeeder.CreateBranch();

        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var command = new DeleteBranchCommand(1);
        
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.ShouldBe(true);
        
    }


    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        
        //Arrange
        var command = new DeleteBranchCommand(1);
        
        
        //Act
        var result =   _handler.Handle(command, CancellationToken.None);

        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        
        
    }
}