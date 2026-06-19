using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.BranchConfigurations.Commands.CreateBranchConfiguration;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchConfigurationTests;

public class CreateBranchConfigurationCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<CreateBranchConfigurationCommandHandler>> _mockLogger;
    private readonly CreateBranchConfigurationCommandHandler _handler;
   

    public CreateBranchConfigurationCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<CreateBranchConfigurationCommandHandler>>();
        _handler = new CreateBranchConfigurationCommandHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Handle_Should_Create_BranchConfiguration()
    {
        // Arrange

        var company = TestDataSeeder.CreateCompany();
        var branch = TestDataSeeder.CreateBranch();
        
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new CreateBranchConfigurationCommand(
            1,
            100,
            new TimeOnly(08,00,00),
            new TimeOnly(18,00,00),
            new TimeOnly(12,00,00),
            new TimeOnly(13,00,00));

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.Id.ShouldBe(1);
        result.MaxTicketsPerDay.ShouldBe(100);
        
        
    }
    
    [Fact]
    public async Task Handle_Should_Return_BranchConfigurationResponse()
    {
        // Arrange

        var company = TestDataSeeder.CreateCompany();
        var branch = TestDataSeeder.CreateBranch();
        
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new CreateBranchConfigurationCommand(
            1,
            100,
            new TimeOnly(08,00,00),
            new TimeOnly(18,00,00),
            new TimeOnly(12,00,00),
            new TimeOnly(13,00,00));

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.Id.ShouldBe(1);
        result.MaxTicketsPerDay.ShouldBe(100);
    }


    [Fact]
    public async Task Should_Return_BranchNotFound()
    {
        var command = new CreateBranchConfigurationCommand(
            1,
            100,
            new TimeOnly(08,00,00),
            new TimeOnly(18,00,00),
            new TimeOnly(12,00,00),
            new TimeOnly(13,00,00));

        // Act

        var result =  _handler.Handle(
            command,
            CancellationToken.None);

        
        //Assert

        var exception = await result.ShouldThrowAsync<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}