using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Branches.Commands.UpdateBranch;
using BranchService.Contracts.Events.BranchEvents;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchTests;

public class UpdateBranchCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<UpdateBranchCommandHandler>> _mockLogger;
    private readonly UpdateBranchCommandHandler _handler;

    public UpdateBranchCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<UpdateBranchCommandHandler>>();
        _handler = new UpdateBranchCommandHandler(_mockLogger.Object, _dbContext, _mockPublishEndpoint.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Branch()
    {
        //Arrange

        var branch = TestDataSeeder.CreateBranch();

        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new UpdateBranchCommand(
            1,
            "Update branch name",
            "Update city",
            "Update Address",
            "+123456789",
            "update@test.com");
        
        
        //Act
        var result =await _handler.Handle(command, CancellationToken.None);
        
        
        //Assert
        result.Should().NotBeNull();
        result.Id.ShouldBe(1);


        var branchResult= await _dbContext.Branches
            .FirstOrDefaultAsync();

        branchResult.Should().NotBeNull();

        branchResult!.BranchName.Should().Be("Update branch name");
    }


    [Fact]
    public  async Task Handle_Should_Return_NotFound()
    {
        var command = new UpdateBranchCommand(
            1,
            "Update branch name",
            "Update city",
            "Update Address",
            "+123456789",
            "update@test.com");
        
        
        //Act
        var result =  _handler.Handle(command, CancellationToken.None);
        
        
        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Handle_Should_Publish_Event()
    {
        //Arrange
        var branch = TestDataSeeder.CreateBranch();

        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new UpdateBranchCommand(
            1,
            "Update branch name",
            "Update city",
            "Update Address",
            "+123456789",
            "update@test.com");
        
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        _mockPublishEndpoint.Verify(x=>
            x.Publish(It.IsAny<BranchUpdatedEvent>(),
                It.IsAny<CancellationToken>()), Times.Once);
        
    }
}