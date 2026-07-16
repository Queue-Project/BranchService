using BranchService.Application.Requests;
using BranchService.Application.Validators.CompanyValidators;
using BranchService.Domain.Enums;
using FluentValidation.TestHelper;

namespace BranchService.UnitTest.BranchService.Application.Tests.ValidatorTests.CompanyValidatorTests;

public class UpdateCompanyValidatorTests
{
     private readonly UpdateCompanyValidator _validator;

    public UpdateCompanyValidatorTests()
    {
        _validator = new UpdateCompanyValidator();
    }

    [Fact]
    public async Task Validator_WhenCommandIsValid_ShouldNotHaveAnyValidationErrors()
    {
        var command = new UpdateCompanyRequest
        {
            CompanyName = "Test Name",
            Address = "Test Address",
            EmailAddress = "test@gmail.com",
            PhoneNumber = "+992923324252",
            CompanyCategory = CompanyCategory.Beauty,
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public async Task Validator_WhenCompanyNameIsEmpty_ShouldHaveValidationError()
    {
        var command = new UpdateCompanyRequest
        {
            CompanyName = "",
            Address = "Test Address",
            EmailAddress = "test@gmail.com",
            PhoneNumber = "+992923324252"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CompanyName)
            .WithErrorMessage("Company Name  is required.");
    }

    [Fact]
    public async Task Validator_WhenAddressIsEmpty_ShouldHaveValidationError()
    {
        var command = new UpdateCompanyRequest
        {
            CompanyName = "Test Name",
            Address = "",
            EmailAddress = "test@gmail.com",
            PhoneNumber = "+992923324252"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.Address)
            .WithErrorMessage("Address is required");
    }

    [Fact]
    public async Task Validator_WhenEmailIsNotCorrect_ShouldHaveValidationError()
    {
        var command = new UpdateCompanyRequest
        {
            CompanyName = "Test Name",
            Address = "Test Address",
            EmailAddress = "testGmail.com",
            PhoneNumber = "+992923324252"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.EmailAddress)
            .WithErrorMessage("Invalid email address format.");
    }

    [Fact]
    public async Task Validator_WhenPhoneNumberIsNotCorrect_ShouldHaveValidationError()
    {
        var command = new UpdateCompanyRequest
        {
            CompanyName = "Test Name",
            Address = "Test Address",
            EmailAddress = "test@gmail.com",
            PhoneNumber = "A92923324252"
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.PhoneNumber)
            .WithErrorMessage("Phone number is invalid. Use formats: +1234567890, (123) 456-7890, or 123-456-7890");
    }
}