using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.CompanyServices.Commands.CreateService;
using BranchService.Contracts.Events.CompanyServiceEvents;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

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

        var company = TestDataSeeder.CreateCompany();
        var branch = TestDataSeeder.CreateBranch();
        await _dbContext.Companies.AddAsync(company);
        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        var command = new CreateServiceCommand(
            1,
            1,
            "Test CompanyService Name",
            "Test CompanyService Description",
            45);

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
        
        
        var company = TestDataSeeder.CreateCompany();
        var branch = TestDataSeeder.CreateBranch();
        await _dbContext.Companies.AddAsync(company);
        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var command = new CreateServiceCommand(
            1,
            1,
            "Test CompanyService Name",
            "Test CompanyService Description",
            45);

        // Act

        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.ServiceName.Should().Be(command.ServiceName);
    }
    
    [Fact]
    public async Task Handle_Should_Return_CompanyNotFound()
    {
        //Arrange
        
        var command = new CreateServiceCommand(
            1,
            1,
            "Test CompanyService Name",
            "Test CompanyService Description",
            45);

        // Act

        var result =  _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe($"Company with Id {command.CompanyId} not found");
    }
    
    [Fact]
    public async Task Handle_Should_Return_BranchNotFound()
    {
        //Arrange
        
        var company = TestDataSeeder.CreateCompany();
        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        var command = new CreateServiceCommand(
            1,
            1,
            "Test CompanyService Name",
            "Test CompanyService Description",
            45);

        // Act

        var result =  _handler.Handle(
            command,
            CancellationToken.None);

        // Assert

        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        exception.Message.ShouldBe($"Branch with Id {command.BranchId} not found");
        
    }

}