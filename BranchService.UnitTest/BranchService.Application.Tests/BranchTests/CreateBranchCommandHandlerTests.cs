using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Branches.Commands.CreateBranch;
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

public class CreateBranchCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<CreateBranchCommandHandler>> _mockLogger;
    private readonly CreateBranchCommandHandler _handler;
   

    public CreateBranchCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<CreateBranchCommandHandler>>();
        _handler = new CreateBranchCommandHandler(_mockLogger.Object, _dbContext, _mockPublishEndpoint.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Branch()
    {
        // Arrange

        var company = TestDataSeeder.CreateCompany();
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new CreateBranchCommand(
            "Test Branch",
            "Test City",
            "Test Branch Address",
            "992923324252",
            "testBranch@test.com",
            1);

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.BranchName.Should().Be("Test Branch");

        var branch = await _dbContext.Branches
            .FirstOrDefaultAsync();

        branch.Should().NotBeNull();

        branch!.City.Should().Be("Test City");

        _mockPublishEndpoint.Verify(
            x => x.Publish(
                It.IsAny<BranchCreatedEvent>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Handle_Should_Return_BranchResponse()
    {
        // Arrange

        var company = TestDataSeeder.CreateCompany();
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        var command = new CreateBranchCommand(
            "Test Branch",
            "Test City",
            "Test Branch Address",
            "992923324252",
            "testBranch@test.com",
            1);

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.BranchName.Should().Be(command.BranchName);
    }

    [Fact]
    public async Task Handle_Should_Return_CompanyNotFound()
    {
        //Arrange
        
        var command = new CreateBranchCommand(
            "Test Branch",
            "Test City",
            "Test Branch Address",
            "992923324252",
            "testBranch@test.com",
            1);

        // Act

        var result =  _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        
    }
}