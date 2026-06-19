using BranchService.Application.UseCases.Companies.Commands.CreateCompany;
using BranchService.Contracts.Events.CompanyEvents;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyTests;

public class CreateCompanyCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<CreateCompanyCommandHandler>> _mockLogger;
    private readonly CreateCompanyCommandHandler _handler;
   

    public CreateCompanyCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<CreateCompanyCommandHandler>>();
        _handler = new CreateCompanyCommandHandler(_mockLogger.Object, _dbContext, _mockPublishEndpoint.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Company()
    {
        // Arrange
        
        var command = new CreateCompanyCommand(
            "Google",
            "California",
            "google@test.com",
            "+123456789");

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.CompanyName.Should().Be("Google");

        var company = await _dbContext.Companies
            .FirstOrDefaultAsync();

        company.Should().NotBeNull();

        company!.CompanyName.Should().Be("Google");

        _mockPublishEndpoint.Verify(
            x => x.Publish(
                It.IsAny<CompanyCreatedEvent>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Handle_Should_Return_CompanyResponse()
    {
        // Arrange

        var command = new CreateCompanyCommand(
            "Google",
            "California",
            "google@test.com",
            "+123456789");

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.CompanyName.Should().Be(command.CompanyName);
    }
    
    
    
    
}