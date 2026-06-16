using BranchService.Application.Interfaces.Data;
using BranchService.Domain.Models;
using BranchService.Infrastructura.Persistence.TableConfiguration;
using Microsoft.EntityFrameworkCore;

namespace BranchService.Infrastructura.Persistence.DataBase;

public class BranchServiceDbContext : DbContext, IBranchServiceApplicationDbContext
{
    public BranchServiceDbContext(DbContextOptions<BranchServiceDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BranchTableConfiguration).Assembly);
    }

    public DbSet<CompanyEntity> Companies { get; set; }
    public DbSet<CompanyServiceEntity> CompanyServices { get; set; }
    public DbSet<BranchEntity> Branches { get; set; }
    public DbSet<BranchConfigurationEntity> BranchConfigurations { get; set; }
}