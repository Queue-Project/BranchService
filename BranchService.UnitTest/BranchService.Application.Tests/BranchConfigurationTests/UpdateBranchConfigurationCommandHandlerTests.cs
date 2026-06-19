using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.BranchConfigurations.Commands.UpdateBranchConfiguration;
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

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchConfigurationTests;

public class UpdateBranchConfigurationCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<UpdateBranchConfigurationCommandHandler>> _mockLogger;
    private readonly UpdateBranchConfigurationCommandHandler _handler;

    public UpdateBranchConfigurationCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<UpdateBranchConfigurationCommandHandler>>();
        _handler = new UpdateBranchConfigurationCommandHandler(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task Handle_Should_Update_BranchConfiguration()
    {
        //Arrange

        // var company = TestDataSeeder.CreateCompany();
        var branch = TestDataSeeder.CreateBranch();
        var branchConfiguration = TestDataSeeder.CreatBranchConfiguration();

        await _dbContext.Branches.AddAsync(branch, CancellationToken.None);
        await _dbContext.BranchConfigurations.AddAsync(branchConfiguration, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new UpdateBranchConfigurationCommand(
            1,
            50,
            new TimeOnly(09,00,00),
            new TimeOnly(19,00,00),
            new TimeOnly(13,00,00),
            new TimeOnly(14,00,00));
        
        
        //Act
        var result =await _handler.Handle(command, CancellationToken.None);
        
        
        //Assert
        result.Should().NotBeNull();
        result.Id.ShouldBe(1);
        result.BranchId.ShouldBe(1);
        result.MaxTicketsPerDay.ShouldBe(50);

    }


    [Fact]
    public  async Task Handle_Should_Return_BranchNotFound()
    {
        var command = new UpdateBranchConfigurationCommand(
            1,
            50,
            new TimeOnly(09,00,00),
            new TimeOnly(19,00,00),
            new TimeOnly(13,00,00),
            new TimeOnly(14,00,00));
        
        
        //Act
        var result =  _handler.Handle(command, CancellationToken.None);
        
        
        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
}