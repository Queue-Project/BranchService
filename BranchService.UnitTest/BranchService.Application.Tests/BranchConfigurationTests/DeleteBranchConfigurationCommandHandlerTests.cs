using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.BranchConfigurations.Commands.DeleteBranchConfiguration;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchConfigurationTests;

public class DeleteBranchConfigurationCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<DeleteBranchConfigurationCommandHandler>> _mockLogger;
    private readonly DeleteBranchConfigurationCommandHandler _handler;

    public DeleteBranchConfigurationCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<DeleteBranchConfigurationCommandHandler>>();
        _handler = new DeleteBranchConfigurationCommandHandler(_mockLogger.Object, _dbContext);
    }


    [Fact]
    public async Task Handle_Should_Delete_BranchConfiguration()
    {
        
        //Arrange
        var branch = TestDataSeeder.CreatBranchConfiguration();

        await _dbContext.BranchConfigurations.AddAsync(branch, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var command = new DeleteBranchConfigurationCommand(1);
        
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.ShouldBe(true);
        
    }


    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        
        //Arrange
        var command = new DeleteBranchConfigurationCommand(1);
        
        
        //Act
        var result =   _handler.Handle(command, CancellationToken.None);

        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        
        
    }
}