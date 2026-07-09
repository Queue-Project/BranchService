using BranchService.Application.Requests;
using BranchService.Application.Validators.BranchValidators;
using FluentValidation.TestHelper;

namespace BranchService.UnitTest.BranchService.Application.Tests.ValidatorTests.BranchValidatorTests;

public class UpdateBranchValidatorTests
{
    private readonly UpdateBranchValidator _validator;

    public UpdateBranchValidatorTests()
    {
        _validator = new UpdateBranchValidator();
    }

    [Fact]
    public async Task Validator_WhenCommandIsValid_ShouldNotHaveAnyValidationErrors()
    {
        var command = new UpdateBranchRequest
        {
            CompanyId = 1,
            BranchName = "Test Name",
            City = "Test City",
            Address = "Test Address",
            PhoneNumber = "+992923324252",
            EmailAddress = "test@branch.com",
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public async Task Validator_WhenBranchNameIsEmpty_ShouldHaveValidationError()
    {
        var command = new UpdateBranchRequest
        {
            CompanyId = 1,
            BranchName = "",
            City = "Test City",
            Address = "Test Address",
            PhoneNumber = "+992923324252",
            EmailAddress = "test@branch.com",
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.BranchName)
            .WithErrorMessage("Branch name is required.");
    }

    [Fact]
    public async Task Validator_WhenAddressIsEmpty_ShouldHaveValidationError()
    {
        var command = new UpdateBranchRequest
        {
            CompanyId = 1,
            BranchName = "Test Name",
            City = "Test City",
            Address = "",
            PhoneNumber = "+992923324252",
            EmailAddress = "test@branch.com",
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.Address)
            .WithErrorMessage("Address is required.");
    }

    [Fact]
    public async Task Validator_WhenCityIsEmpty_ShouldHaveValidationError()
    {
        var command = new UpdateBranchRequest
        {
            CompanyId = 1,
            BranchName = "Test Name",
            City = "",
            Address = "Test Address",
            PhoneNumber = "+992923324252",
            EmailAddress = "test@branch.com",
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.City)
            .WithErrorMessage("City is required");
    }

    [Fact]
    public async Task Validator_WhenEmailIsNotCorrect_ShouldHaveValidationError()
    {
        var command = new UpdateBranchRequest
        {
            CompanyId = 1,
            BranchName = "Test Name",
            City = "Test City",
            Address = "Test Address",
            PhoneNumber = "+992923324252",
            EmailAddress = "testGmail.com",
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.EmailAddress)
            .WithErrorMessage("Invalid email address format.");
    }

    [Fact]
    public async Task Validator_WhenPhoneNumberIsNotCorrect_ShouldHaveValidationError()
    {
        var command = new UpdateBranchRequest
        {
            CompanyId = 1,
            BranchName = "Test Name",
            City = "Test City",
            Address = "Test Address",
            PhoneNumber = "A92923324252",
            EmailAddress = "test@branch.com",
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.PhoneNumber)
            .WithErrorMessage("Phone number is invalid. Use formats: +1234567890, (123) 456-7890, or 123-456-7890");
    }

    [Theory]
    [InlineData(0)]
    public void Validator_WhenCompanyIdIsInvalid_ShouldHaveValidationError(int invalidCompanyId)
    {
        var command = new UpdateBranchRequest
        {
            CompanyId = invalidCompanyId,
            BranchName = "Test Name",
            City = "Test City",
            Address = "Test Address",
            PhoneNumber = "+992923324252",
            EmailAddress = "test@branch.com",
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.CompanyId)
            .WithErrorMessage("CompanyId is required.");
    }
}