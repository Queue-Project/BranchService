using BranchService.Application.UseCases.Companies.Commands.CreateCompany;
using BranchService.Application.UseCases.CompanyServices.Commands.CreateService;
using BranchService.Contracts.Events.CompanyEvents;
using BranchService.Contracts.Events.CompanyServiceEvents;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyServiceTests;

public class CreateCompanyServiceCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<CreateServiceCommandHandler>> _mockLogger;
    private readonly CreateServiceCommandHandler _handler;
   

    public CreateCompanyServiceCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<CreateServiceCommandHandler>>();
        _handler = new CreateServiceCommandHandler(_mockLogger.Object, _dbContext, _mockPublishEndpoint.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_CompanyService()
    {
        // Arrange
        
        var command = new CreateServiceCommand(
            1,
            "Test CompanyService Name",
            "Test CompanyService Description");

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.ServiceName.Should().Be("Test CompanyService Name");
        
        _mockPublishEndpoint.Verify(
            x => x.Publish(
                It.IsAny<CompanyServiceCreatedEvent>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Handle_Should_Return_CompanyServiceResponse()
    {
        // Arrange

        var command = new CreateServiceCommand(
            1,
            "Test CompanyService Name",
            "Test CompanyService Description");

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.ServiceName.Should().Be(command.ServiceName);
    }

}