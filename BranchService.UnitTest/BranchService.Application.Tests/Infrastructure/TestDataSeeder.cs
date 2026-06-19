using BranchService.Domain.Models;

namespace BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;

public static class TestDataSeeder
{
    public static CompanyEntity CreateCompany()
    {
        return new CompanyEntity
        {
            Id = 1,
            CompanyName = "Test Company",
            Address = "Test Address",
            EmailAddress = "test@test.com",
            PhoneNumber = "+992000000000",
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public static List<CompanyEntity> CreateCompanies()
    {
        return new List<CompanyEntity>
        {
            CreateCompany(),
            new CompanyEntity
            {
                Id = 2,
                CompanyName = "Test Company2",
                Address = "Test Address2",
                EmailAddress = "test2@test.com",
                PhoneNumber = "+992100000000",
                CreatedAt = DateTimeOffset.UtcNow
            },
            new CompanyEntity
            {
                Id = 3,
                CompanyName = "Test Company3",
                Address = "Test Address3",
                EmailAddress = "test3@test.com",
                PhoneNumber = "+992300000000",
                CreatedAt = DateTimeOffset.UtcNow
            }
        };
    }

    public static BranchEntity CreateBranch()
    {
        return new BranchEntity
        {
            Id = 1,
            CompanyId = 1,
            BranchName = "Test Branch",
            City = "Test City",
            Address = "Test Branch Address",
            EmailAddress = "testBranch@test.com",
            PhoneNumber = "+992923324252",
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public static List<BranchEntity> CreateBranches()
    {
        return new List<BranchEntity>
        {
            CreateBranch(),
            new BranchEntity
            {
                Id = 2,
                CompanyId = 1,
                BranchName = "Test Branch2",
                City = "Test City2",
                Address = "Test Branch Address2",
                EmailAddress = "testBranch2@test.com",
                PhoneNumber = "+992923324251",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow
            },
            new BranchEntity
            {
                Id = 3,
                CompanyId = 1,
                BranchName = "Test Branch3",
                City = "Test City3",
                Address = "Test Branch Address3",
                EmailAddress = "testBranch3@test.com",
                PhoneNumber = "+992923324253",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow
            }
        };
    }

    public static BranchConfigurationEntity CreatBranchConfiguration()
    {
        return new BranchConfigurationEntity
        {
            Id = 1,
            BranchId = 1,
            OpenTime = new TimeOnly(09, 00, 00),
            CloseTime = new TimeOnly(18, 00, 00),
            BreakStartTime = new TimeOnly(13, 00, 00),
            BreakEndTime = new TimeOnly(14, 00, 00),
            MaxTicketsPerDay = 50,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public static List<BranchConfigurationEntity> CreateBranchConfigurations()
    {
        return new List<BranchConfigurationEntity>
        {
            CreatBranchConfiguration(),
            new BranchConfigurationEntity
            {
                Id = 2,
                BranchId = 2,
                OpenTime = new TimeOnly(09, 00, 00),
                CloseTime = new TimeOnly(19, 00, 00),
                BreakStartTime = new TimeOnly(13, 00, 00),
                BreakEndTime = new TimeOnly(14, 00, 00),
                MaxTicketsPerDay = 80,
                CreatedAt = DateTimeOffset.UtcNow
            },
            new BranchConfigurationEntity
            {
                Id = 3,
                BranchId = 3,
                OpenTime = new TimeOnly(08, 00, 00),
                CloseTime = new TimeOnly(19, 00, 00),
                BreakStartTime = new TimeOnly(12, 00, 00),
                BreakEndTime = new TimeOnly(14, 00, 00),
                MaxTicketsPerDay = 70,
                CreatedAt = DateTimeOffset.UtcNow
            }
        };
    }

    public static CompanyServiceEntity CreateCompanyService()
    {
        return new CompanyServiceEntity
        {
            Id = 1,
            CompanyId = 1,
            ServiceName = "Test CompanyService Name",
            ServiceDescription = "Test CompanyService Description",
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public static List<CompanyServiceEntity> CreateCompanyServices()
    {
        return new List<CompanyServiceEntity>
        {
            CreateCompanyService(),
            new CompanyServiceEntity
            {
                Id = 2,
                CompanyId = 1,
                ServiceName = "Test CompanyService Name2",
                ServiceDescription = "Test CompanyService Description2",
                CreatedAt = DateTimeOffset.UtcNow
            },
            new CompanyServiceEntity
            {
                Id = 3,
                CompanyId = 1,
                ServiceName = "Test CompanyService Name3",
                ServiceDescription = "Test CompanyService Description3",
                CreatedAt = DateTimeOffset.UtcNow
            }
        };
    }
}