using System.Net;
using System.Net.Http.Json;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Companies.Commands.CreateCompany;
using BranchService.Domain.Enums;
using Shouldly;
using Xunit;

namespace BranchService.IntegrationTest.Controllers;

public class CompanyControllerTest : IClassFixture<QBranchServiceWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly QBranchServiceWebApplicationFactory _factory;

    public CompanyControllerTest(QBranchServiceWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateCompany_ShouldReturnCreatedCompany_WithValidRequest()
    {
        var createCompanyCommand = new CreateCompanyCommand(
            CompanyName: "TestCompany",
            Address: "TestAddress",
            EmailAddress: "test@gmail.com",
            PhoneNumber: "+992921111112",
            CompanyCategory.Beauty);

        var response = await _client.PostAsJsonAsync("/api/Company", createCompanyCommand);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CompanyResponseModel>();
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
    }

    [Fact]
    public async Task GetCompany_ExistingCompany_ReturnsSuccess()
    {
        var createCompanyCommand = new CreateCompanyCommand(
            CompanyName: "TestCompany",
            Address: "TestAddress",
            EmailAddress: "test@gmail.com",
            PhoneNumber: "+992921111112",
            CompanyCategory.Beauty);

        var companyCreatedResponse = await _client.PostAsJsonAsync("/api/Company", createCompanyCommand);

        companyCreatedResponse.EnsureSuccessStatusCode();
        var createdCompanyResult = await companyCreatedResponse.Content.ReadFromJsonAsync<CompanyResponseModel>();
        createdCompanyResult.ShouldNotBeNull();
        createdCompanyResult.Id.ShouldNotBe(0);
        var companyId = createdCompanyResult.Id;
        var response = await _client.GetAsync($"api/Company/{companyId}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CompanyResponseModel>();
        result.ShouldNotBeNull();
        result.Id.ShouldBe(companyId);
    }

    [Fact]
    public async Task GetCompany_NonExistentCompany_ReturnsNotFound()
    {
        var nonExistentCompanyId = 999;
        var response = await _client.GetAsync($"api/Company/{nonExistentCompanyId}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}