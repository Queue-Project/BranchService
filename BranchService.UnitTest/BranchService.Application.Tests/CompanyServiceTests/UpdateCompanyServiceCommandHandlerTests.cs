using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.CompanyServices.Commands.UpdateService;
using BranchService.Contracts.Events.CompanyServiceEvents;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyServiceTests;

public class UpdateCompanyServiceCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<UpdateServiceCommandHandler>> _mockLogger;
    private readonly UpdateServiceCommandHandler _handler;

    public UpdateCompanyServiceCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<UpdateServiceCommandHandler>>();
        _handler = new UpdateServiceCommandHandler(_mockLogger.Object, _dbContext, _mockPublishEndpoint.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_CompanyService()
    {
        //Arrange

        var company = TestDataSeeder.CreateCompany();
        var companyService = TestDataSeeder.CreateCompanyService();

        await _dbContext.CompanyServices.AddAsync(companyService, CancellationToken.None);
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new UpdateServiceCommand(
            1,
            "Update CompanyService Name",
            "Update CompanyService Description",
            45);
        
        
        //Act
        var result =await _handler.Handle(command, CancellationToken.None);
        
        
        //Assert
        result.Should().NotBeNull();
        result.Id.ShouldBe(1);
        result.CompanyId.ShouldBe(1);
        result.ServiceName.ShouldBe("Update CompanyService Name");
        
    }


    [Fact]
    public  async Task Handle_Should_Return_NotFound()
    {
        var command = new UpdateServiceCommand(
            1,
            "Update CompanyService Name",
            "Update CompanyService Description",
            45);
        
        
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
        var company = TestDataSeeder.CreateCompany();
        var companyService = TestDataSeeder.CreateCompanyService();

        await _dbContext.CompanyServices.AddAsync(companyService, CancellationToken.None);
        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new UpdateServiceCommand(
            1,
            "Update CompanyService Name",
            "Update CompanyService Description",
            45);
        
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        _mockPublishEndpoint.Verify(x=>
            x.Publish(It.IsAny<CompanyServiceUpdatedEvent>(),
                It.IsAny<CancellationToken>()), Times.Once);
        
    }

    

    [Fact]
    public async Task Handle_Should_Return_ServiceNotFound()
    {
        //Arrange
        
        var company = TestDataSeeder.CreateCompany();

        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var command = new UpdateServiceCommand(
            1,
            "Update CompanyService Name",
            "Update CompanyService Description",
            45);
        
        //Act
        var result =  _handler.Handle(command, CancellationToken.None);
        
        
        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}