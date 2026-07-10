using System.Net;
using System.Net.Http.Json;
using BranchService.Application.Response;
using BranchService.Application.UseCases.Branches.Commands.CreateBranch;
using BranchService.Application.UseCases.Companies.Commands.CreateCompany;
using BranchService.Application.UseCases.CompanyServices.Commands.CreateService;
using BranchService.Domain.Enums;
using Shouldly;
using Xunit;

namespace BranchService.IntegrationTest.Controllers;

public class CompanyServiceControllerTest : IClassFixture<QBranchServiceWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly QBranchServiceWebApplicationFactory _factory;

    public CompanyServiceControllerTest(QBranchServiceWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateCompanyService_ShouldReturnCreatedCompanyService_WithValidRequest()
    {
        var createCompanyCommand = new CreateCompanyCommand(
            CompanyName: "TestCompany",
            Address: "TestAddress",
            EmailAddress: "test@gmail.com",
            PhoneNumber: "+992921111112",
            CompanyCategory.Beauty);

        var companyResponse = await _client.PostAsJsonAsync("/api/Company", createCompanyCommand);

        companyResponse.EnsureSuccessStatusCode();
        var companyResult = await companyResponse.Content.ReadFromJsonAsync<CompanyResponseModel>();
        companyResult.ShouldNotBeNull();
        companyResult.Id.ShouldNotBe(0);

        var createBranchCommand = new CreateBranchCommand(
            CompanyId: companyResult.Id,
            BranchName: "TestBranchName",
            Address: "TestAddress",
            City: "TestCity",
            EmailAddress: "test@gmail.com",
            PhoneNumber: "+992981111112");

        var branchResponse = await _client.PostAsJsonAsync("/api/Branch", createBranchCommand);
        branchResponse.EnsureSuccessStatusCode();
        var branchResult = await branchResponse.Content.ReadFromJsonAsync<BranchResponseModel>();
        branchResult.ShouldNotBeNull();
        branchResult.Id.ShouldNotBe(0);


        var createCompanyService = new CreateServiceCommand(
            CompanyId: companyResult.Id,
            BranchId: branchResult.Id,
            ServiceName: "TestServiceName",
            ServiceDescription: "TestServiceDescription",
            ServiceDuration: 45);

        var response = await _client.PostAsJsonAsync("/api/CompanyService", createCompanyService);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CompanyServiceResponseModel>();
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
    }

    [Fact]
    public async Task GetCompanyService_ExistingCompanyService_ReturnsSuccess()
    {
        var createCompanyCommand = new CreateCompanyCommand(
            CompanyName: "TestCompany",
            Address: "TestAddress",
            EmailAddress: "test@gmail.com",
            PhoneNumber: "+992921111112",
            CompanyCategory.Beauty);

        var companyCreatedResponse = await _client.PostAsJsonAsync("/api/Company", createCompanyCommand);

        companyCreatedResponse.EnsureSuccessStatusCode();
        var companyCreatedResult = await companyCreatedResponse.Content.ReadFromJsonAsync<CompanyResponseModel>();
        companyCreatedResult.ShouldNotBeNull();
        companyCreatedResult.Id.ShouldNotBe(0);

        var createBranchCommand = new CreateBranchCommand(
            CompanyId: companyCreatedResult.Id,
            BranchName: "TestBranchName",
            Address: "TestAddress",
            City: "TestCity",
            EmailAddress: "test@gmail.com",
            PhoneNumber: "+992981111112");

        var branchResponse = await _client.PostAsJsonAsync("/api/Branch", createBranchCommand);
        branchResponse.EnsureSuccessStatusCode();
        var branchResult = await branchResponse.Content.ReadFromJsonAsync<BranchResponseModel>();
        branchResult.ShouldNotBeNull();
        branchResult.Id.ShouldNotBe(0);

        var createCompanyService = new CreateServiceCommand(
            CompanyId: companyCreatedResult.Id,
            BranchId: branchResult.Id,
            ServiceName: "TestServiceName",
            ServiceDescription: "TestServiceDescription",
            ServiceDuration: 45);

        var companyServiceCreatedResponse = await _client.PostAsJsonAsync("/api/CompanyService", createCompanyService);
        companyServiceCreatedResponse.EnsureSuccessStatusCode();
        var companyServiceCreatedResult =
            await companyServiceCreatedResponse.Content.ReadFromJsonAsync<CompanyServiceResponseModel>();
        companyServiceCreatedResult.ShouldNotBeNull();
        companyServiceCreatedResult.Id.ShouldNotBe(0);


        var companyServiceId = companyServiceCreatedResult.Id;
        var response = await _client.GetAsync($"api/CompanyService/{companyServiceId}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CompanyServiceResponseModel>();
        result.ShouldNotBeNull();
        result.Id.ShouldBe(companyServiceId);
    }

    [Fact]
    public async Task GetCompanyService_NonExistentCompanyService_ReturnsNotFound()
    {
        var nonExistentCompanyServiceId = 999;
        var response = await _client.GetAsync($"api/CompanyService/{nonExistentCompanyServiceId}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}