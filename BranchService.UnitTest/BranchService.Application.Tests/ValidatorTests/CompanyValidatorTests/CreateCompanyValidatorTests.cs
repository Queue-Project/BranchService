using BranchService.Application.UseCases.Companies.Commands.CreateCompany;
using BranchService.Application.Validators.CompanyValidators;
using BranchService.Domain.Enums;
using FluentValidation.TestHelper;

namespace BranchService.UnitTest.BranchService.Application.Tests.ValidatorTests.CompanyValidatorTests;

public class CreateCompanyValidatorTests
{
    private readonly CreateCompanyValidator _validator;

    public CreateCompanyValidatorTests()
    {
        _validator = new CreateCompanyValidator();
    }

    [Fact]
    public async Task Validator_WhenCommandIsValid_ShouldNotHaveAnyValidationErrors()
    {
        var command = new CreateCompanyCommand(
            "Test Name",
            "Test Address",
            "test@company.com",
            "+992923324252",
            CompanyCategory.Beauty);

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public async Task Validator_WhenCompanyNameIsEmpty_ShouldHaveValidationError()
    {
        var command = new CreateCompanyCommand(
            "",
            "Test Address",
            "test@company.com",
            "+992923324252",
            CompanyCategory.Beauty);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CompanyName)
            .WithErrorMessage("Company Name  is required.");
    }

    [Fact]
    public async Task Validator_WhenAddressIsEmpty_ShouldHaveValidationError()
    {
        var command = new CreateCompanyCommand(
            "Test Name",
            "",
            "test@company.com",
            "+992923324252",
            CompanyCategory.Beauty);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.Address)
            .WithErrorMessage("Address is required");
    }

    [Fact]
    public async Task Validator_WhenEmailIsNotCorrect_ShouldHaveValidationError()
    {
        var command = new CreateCompanyCommand(
            "Test Name",
            "Test Address",
            "testCompany.com",
            "+992923324252",
            CompanyCategory.Beauty);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.EmailAddress)
            .WithErrorMessage("Invalid email address format.");
    }

    [Fact]
    public async Task Validator_WhenPhoneNumberIsNotCorrect_ShouldHaveValidationError()
    {
        var command = new CreateCompanyCommand(
            "Test Name",
            "Test Address",
            "test@company.com",
            "A92923324252",
            CompanyCategory.Beauty);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(s => s.PhoneNumber)
            .WithErrorMessage("Phone number is invalid. Use formats: +1234567890, (123) 456-7890, or 123-456-7890");
    }
}