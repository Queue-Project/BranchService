using BranchService.Application.UseCases.Branches.Commands.CreateBranch;
using BranchService.Application.Validators.BranchValidators;
using FluentValidation.TestHelper;


namespace BranchService.UnitTest.BranchService.Application.Tests.ValidatorTests.BranchValidatorTests;

public class CreateBranchValidatorTests
{
    private readonly CreateBranchValidator _validator;

    public CreateBranchValidatorTests()
    {
        _validator = new CreateBranchValidator();
    }

    [Fact]
    public async Task Validator_WhenCommandIsValid_ShouldNotHaveAnyValidationErrors()
    {
        var command = new CreateBranchCommand(
            "Test Name",
            "Test City",
            "Test Address",
            "+992923324252",
            "test@branch.com",
            1);

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public async Task Validator_WhenBranchNameIsEmpty_ShouldHaveValidationError()
    {
        var command = new CreateBranchCommand(
            "",
            "Test City",
            "Test Address",
            "+992923324252",
            "test@branch.com",
            1);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.BranchName)
            .WithErrorMessage("Branch name is required.");
    }

    [Fact]
    public async Task Validator_WhenAddressIsEmpty_ShouldHaveValidationError()
    {
        var command = new CreateBranchCommand(
            "Test Name",
            "Test City",
            "",
            "+992923324252",
            "test@branch.com",
            1);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.Address)
            .WithErrorMessage("Address is required.");
    }

    [Fact]
    public async Task Validator_WhenCityIsEmpty_ShouldHaveValidationError()
    {
        var command = new CreateBranchCommand(
            "Test Name",
            "",
            "Test Address",
            "+992923324252",
            "test@branch.com",
            1);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.City)
            .WithErrorMessage("City is required");
    }

    [Fact]
    public async Task Validator_WhenEmailIsNotCorrect_ShouldHaveValidationError()
    {
        var command = new CreateBranchCommand(
            "Test Name",
            "Test City",
            "Test Address",
            "+992923324252",
            "testGmail.com",
            1);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.EmailAddress)
            .WithErrorMessage("Invalid email address format.");
    }

    [Fact]
    public async Task Validator_WhenPhoneNumberIsNotCorrect_ShouldHaveValidationError()
    {
        var command = new CreateBranchCommand(
            "Test Name",
            "Test City",
            "Test Address",
            "A92923324252",
            "test@mail.com",
            1);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.PhoneNumber)
            .WithErrorMessage("Phone number is invalid. Use formats: +1234567890, (123) 456-7890, or 123-456-7890");
    }

    [Theory]
    [InlineData(0)]
    public void Validator_WhenCompanyIdIsInvalid_ShouldHaveValidationError(int invalidCompanyId)
    {
        var command = new CreateBranchCommand(
            "Test Name",
            "Test City",
            "Test Address",
            "+92923324252",
            "test@mail.com",
            invalidCompanyId);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.CompanyId)
            .WithErrorMessage("CompanyId is required.");
    }
}
