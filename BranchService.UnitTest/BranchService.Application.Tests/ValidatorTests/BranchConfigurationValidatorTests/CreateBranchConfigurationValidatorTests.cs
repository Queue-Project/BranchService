using BranchService.Application.UseCases.BranchConfigurations.Commands.CreateBranchConfiguration;
using BranchService.Application.Validators.BranchConfigurationValidator;
using FluentValidation.TestHelper;

namespace BranchService.UnitTest.BranchService.Application.Tests.ValidatorTests.BranchConfigurationValidatorTests;

public class CreateBranchConfigurationValidatorTests
{
    private readonly CreateBranchConfigurationValidator _validator;

    public CreateBranchConfigurationValidatorTests()
    {
        _validator = new CreateBranchConfigurationValidator();
    }
    

    [Fact]
    public void Validator_WhenCommandIsValid_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var command = new CreateBranchConfigurationCommand
        (
            1,
            50,
            new TimeOnly(9, 0),
            new TimeOnly(18, 0),
            new TimeOnly(13, 0),
            new TimeOnly(14, 0)
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_WhenNoBreakTimeProvided_ShouldStillBeValid()
    {
        // Arrange
        var command = new CreateBranchConfigurationCommand
        (
            1,
            50,
            new TimeOnly(8, 0),
            new TimeOnly(17, 0),
            null,
            null
        );

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }



    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Validator_WhenBranchIdIsInvalid_ShouldHaveValidationError(int invalidBranchId)
    {
        // Arrange
        var command = new CreateBranchConfigurationCommand(
            invalidBranchId,
            50,
            new TimeOnly(8, 0),
            new TimeOnly(17, 0),
            null,
            null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BranchId);
    }

    [Fact]
    public void Validator_WhenCloseTimeIsBeforeOpenTime_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateBranchConfigurationCommand
        (
            1,
            50,
            new TimeOnly(17, 0),
            new TimeOnly(9, 0),
            null,
            null
        );

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
        var command = new CreateBranchConfigurationCommand
        (
            1,
            10,
            new TimeOnly(9, 0),
            new TimeOnly(18, 0),
            new TimeOnly(8, 0), // Хато: Пеш аз соати кушодашавӣ аст
            new TimeOnly(14, 0)
        );

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
        var command = new CreateBranchConfigurationCommand
        (
            1,
            10,
            new TimeOnly(9, 0),
            new TimeOnly(18, 0),
            new TimeOnly(13, 0),
            new TimeOnly(19, 0) 
        );

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
        var command = new CreateBranchConfigurationCommand
        (
            1,
            10,
            new TimeOnly(9, 0),
            new TimeOnly(18, 0),
            new TimeOnly(14, 0),
            new TimeOnly(13, 0) 
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BreakEndTime)
            .WithErrorMessage("BreakEndTime must be after BreakStartTime.");
    }

 
}