using System.Net;
using BranchService.Application.Exceptions;
using BranchService.Application.UseCases.CompanyServices.Commands.DeleteService;
using BranchService.Infrastructure.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace BranchService.UnitTest.BranchService.Application.Tests.CompanyServiceTests;

public class DeleteCompanyServiceCommandHandlerTests
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly Mock<ILogger<DeleteServiceCommandHandler>> _mockLogger;
    private readonly DeleteServiceCommandHandler _handler;

    public DeleteCompanyServiceCommandHandlerTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<DeleteServiceCommandHandler>>();
        _handler = new DeleteServiceCommandHandler(_mockLogger.Object, _dbContext, _mockPublishEndpoint.Object);
    }


    [Fact]
    public async Task Handle_Should_Delete_CompanyService()
    {
        
        //Arrange
        var companyService = TestDataSeeder.CreateCompanyService();

        await _dbContext.CompanyServices.AddAsync(companyService, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var command = new DeleteServiceCommand(1);
        
        
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.ShouldBe(true);
        
    }


    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        
        //Arrange
        var command = new DeleteServiceCommand(1);
        
        
        //Act
        var result =   _handler.Handle(command, CancellationToken.None);

        //Assert
        var exception = result.ShouldThrow<HttpStatusCodeException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        
        
    }
}