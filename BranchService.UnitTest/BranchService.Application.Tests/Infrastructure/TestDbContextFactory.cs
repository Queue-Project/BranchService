using BranchService.Infrastructura.Persistence.DataBase;
using Microsoft.EntityFrameworkCore;

namespace BranchService.UnitTest.BranchService.Application.Tests.Infrastructure;

public static class TestDbContextFactory
{
    public static BranchServiceDbContext Create()
    {
        var options = new DbContextOptionsBuilder<BranchServiceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new BranchServiceDbContext(options);

        context.Database.EnsureCreated();

        return context;
    }
}