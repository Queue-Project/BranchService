using BranchService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BranchService.Infrastructura.Persistence.TableConfiguration;

public class BranchConfigurationTableConfiguration: IEntityTypeConfiguration<BranchConfigurationEntity>
{
    public void Configure(EntityTypeBuilder<BranchConfigurationEntity> builder)
    {
        builder.ToTable("BranchConfigurations");
        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.Branch)
            .WithMany()
            .HasForeignKey(c => c.BranchId);
    }
}