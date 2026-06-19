using BranchService.Contracts.Requests;
using BranchService.Infrastructura.Persistence.DataBase;
using BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace BranchService.UnitTest.BranchService.Application.Tests.BranchServiceTest;

public class ValidateQueueCreationTests 
{
    private readonly BranchServiceDbContext _dbContext;
    private readonly Mock<ILogger<global::BranchService.Application.Services.BranchService>> _mockLogger;
    private readonly global::BranchService.Application.Services.BranchService _branchService;

    public ValidateQueueCreationTests()
    {
        _dbContext = TestDbContextFactory.Create();
        _mockLogger = new Mock<ILogger<global::BranchService.Application.Services.BranchService>>();
        _branchService = new global::BranchService.Application.Services.BranchService(_mockLogger.Object, _dbContext);
    }

    [Fact]
    public async Task ValidateQueueCreation_WhenConfigDoesNotExist_ReturnsInvalidResponse()
    {
        // Arrange
        var request = new QueueCreationValidationRequest
        {
            RequestId = Guid.NewGuid(),
            BranchId = 999, 
            RequestedStartTime = DateTimeOffset.UtcNow
        };

        // Act
        var result = await _branchService.ValidateQueueCreationAsync(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("Branch configuration not found");
    }

    [Fact]
    public async Task ValidateQueueCreation_WhenWithinWorkingHoursAndNoBreak_ReturnsValidResponse()
    {
        // Arrange
        var branch = TestDataSeeder.CreateBranch();
        var config = TestDataSeeder.CreatBranchConfiguration();
        await _dbContext.BranchConfigurations.AddAsync(config);
        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

        var requestTime = new DateTimeOffset(2026, 6, 18, 10, 30, 0, TimeSpan.Zero);
        var request = new QueueCreationValidationRequest
        {
            RequestId = Guid.NewGuid(),
            BranchId = branch.Id,
            RequestedStartTime = requestTime
        };

        // Act
        var result = await _branchService.ValidateQueueCreationAsync(request);

        // Assert
        result.IsValid.ShouldBeTrue();
        result.IsWithinWorkingHours.ShouldBeTrue();
        result.IsWithinBreakTime.ShouldBeFalse();
        result.ErrorMessage.ShouldBeNull();
        result.MaxTicketsPerDay.ShouldBe(50);
    }

    [Fact]
    public async Task ValidateQueueCreation_WhenOutsideWorkingHours_ReturnsInvalidResponseWithCorrectMessage()
    {
        // Arrange
        var branch = TestDataSeeder.CreateBranch();
        var config = TestDataSeeder.CreatBranchConfiguration();
        await _dbContext.BranchConfigurations.AddAsync(config);
        await _dbContext.Branches.AddAsync(branch);
        await _dbContext.SaveChangesAsync();

        var requestTime = new DateTimeOffset(2026, 6, 18, 8, 0, 0, TimeSpan.Zero);
        var request = new QueueCreationValidationRequest
        {
            RequestId = Guid.NewGuid(),
            BranchId = branch.Id,
            RequestedStartTime = requestTime
        };

        // Act
        var result = await _branchService.ValidateQueueCreationAsync(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.IsWithinWorkingHours.ShouldBeFalse();
        result.ErrorMessage!.ShouldContain("Branch hours: 09:00 - 18:00");
    }

    
}
