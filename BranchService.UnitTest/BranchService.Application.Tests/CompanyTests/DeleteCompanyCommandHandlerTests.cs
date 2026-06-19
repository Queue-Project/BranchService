using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.Companies.Commands.DeleteCompany;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyTests;

public class DeleteCompanyCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<DeleteCompanyCommandHandler>> _mockLogger;
    private readonly DeleteCompanyCommandHandler _handler;

    public DeleteCompanyCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<DeleteCompanyCommandHandler>>();
        _handler = new DeleteCompanyCommandHandler(_mockLogger.Object, _dbContext, _mockPublishEndpoint.Object);
    }


    [Fact]
    public async Task Handle_Should_Delete_Company()
    {
        
        //Arrange
        var company = TestDataSeeder.CreateCompany();

        await _dbContext.Companies.AddAsync(company, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var command = new DeleteCompanyCommand(1);
        
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.ShouldBe(true);
        
    }


    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        
        //Arrange
        var command = new DeleteCompanyCommand(1);
        
        
        //Act
        var result =   _handler.Handle(command, CancellationToken.None);

        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        
        
    }
}