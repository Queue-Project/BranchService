using BranchService.Application.Requests;
using BranchService.Application.Validators.BranchConfigurationValidator;
using FluentValidation.TestHelper;

namespace BranchService.UnitTest.BranchService.Application.Tests.ValidatorTests.BranchConfigurationValidatorTests;

public class UpdateBranchConfigurationValidatorTests
{
    private readonly UpdateBranchConfigurationValidator _validator;

    public UpdateBranchConfigurationValidatorTests()
    {
        _validator = new UpdateBranchConfigurationValidator();
    }


    [Fact]
    public void Validator_WhenCommandIsValid_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var command = new UpdateBranchConfigurationRequest
        {
            MaxTicketsPerDay = 50,
            OpenTime = new TimeOnly(9, 0),
            CloseTime = new TimeOnly(18, 0),
            BreakStartTime = new TimeOnly(13, 0),
            BreakEndTime = new TimeOnly(14, 0)
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_WhenNoBreakTimeProvided_ShouldStillBeValid()
    {
        // Arrange
        var command = new UpdateBranchConfigurationRequest
        {
            MaxTicketsPerDay = 50,
            OpenTime = new TimeOnly(8, 0),
            CloseTime = new TimeOnly(17, 0),
            BreakStartTime = null,
            BreakEndTime = null
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public void Validator_WhenCloseTimeIsBeforeOpenTime_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateBranchConfigurationRequest
        {
            OpenTime = new TimeOnly(17, 0),
            CloseTime = new TimeOnly(9, 0)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CloseTime)
            .WithErrorMessage("CloseTime must be after OpenTime");
    }

    [Fact]
    public void Validator_WhenBreakStartTimeIsOutsideWorkingHours_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateBranchConfigurationRequest
        {
            MaxTicketsPerDay = 10,
            OpenTime = new TimeOnly(9, 0),
            CloseTime = new TimeOnly(18, 0),
            BreakStartTime = new TimeOnly(8, 0),
            BreakEndTime = new TimeOnly(14, 0)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BreakStartTime)
            .WithErrorMessage("BreakStartTime must be after or equal to OpenTime.");
    }

    [Fact]
    public void Validator_WhenBreakEndTimeIsAfterCloseTime_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateBranchConfigurationRequest
        {
            MaxTicketsPerDay = 10,
            OpenTime = new TimeOnly(9, 0),
            CloseTime = new TimeOnly(18, 0),
            BreakStartTime = new TimeOnly(13, 0),
            BreakEndTime = new TimeOnly(19, 0)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BreakEndTime)
            .WithErrorMessage("BreakEndTime must be before or equal to CloseTime.");
    }

    [Fact]
    public void Validator_WhenBreakEndTimeIsBeforeBreakStartTime_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateBranchConfigurationRequest
        {
            MaxTicketsPerDay = 10,
            OpenTime = new TimeOnly(9, 0),
            CloseTime = new TimeOnly(18, 0),
            BreakStartTime = new TimeOnly(14, 0),
            BreakEndTime = new TimeOnly(13, 0)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BreakEndTime)
            .WithErrorMessage("BreakEndTime must be after BreakStartTime.");
    }
}