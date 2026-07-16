using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Companies.Commands.UpdateCompany;
using BranchService.Contracts.Events.CompanyEvents;
using BranchService.Contracts.Events.Enums;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using FluentAssertions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using CompanyCategory = BranchService.Domain.Enums.CompanyCategory;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyTests;

public class UpdateCompanyCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<UpdateCompanyCommandHandler>> _mockLogger;
    private readonly UpdateCompanyCommandHandler _handler;

    public UpdateCompanyCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<UpdateCompanyCommandHandler>>();
        _handler = new UpdateCompanyCommandHandler(_mockLogger.Object, _dbContext, _mockPublishEndpoint.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Company()
    {
        //Arrange

        var company = TestDataSeeder.CreateCompany();

        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new UpdateCompanyCommand(
            1,
            "Google",
            "California",
            "google@test.com",
            "+123456789",
            CompanyCategory.Beauty
            );
        
        
        //Act
        var result =await _handler.Handle(command, CancellationToken.None);
        
        
        //Assert
        result.Should().NotBeNull();
        result.Id.ShouldBe(1);


        var companyResult= await _dbContext.Companies
            .FirstOrDefaultAsync();

        companyResult.Should().NotBeNull();

        companyResult!.CompanyName.Should().Be("Google");
    }


    [Fact]
    public  async Task Handle_Should_Return_NotFound()
    {
        var command = new UpdateCompanyCommand(
            1,
            "Google",
            "California",
            "google@test.com",
            "+123456789",
            CompanyCategory.Beauty
            );
        
        
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

        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        
        
        var command = new UpdateCompanyCommand(
            1,
            "Google",
            "California",
            "google@test.com",
            "+123456789",
            CompanyCategory.Beauty
            );
        
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        _mockPublishEndpoint.Verify(x=>
            x.Publish(It.IsAny<CompanyUpdatedEvent>(),
                It.IsAny<CancellationToken>()), Times.Once);
        
    }
}