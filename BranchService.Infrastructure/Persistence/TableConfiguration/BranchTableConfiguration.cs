using BranchService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BranchService.Infrastructure.Persistence.TableConfiguration;

public class BranchTableConfiguration: IEntityTypeConfiguration<BranchEntity>
{
    public void Configure(EntityTypeBuilder<BranchEntity> builder)
    {
        builder.ToTable("Branches");
        builder.HasKey(s => s.Id);
        builder.HasOne(s => s.Company)
            .WithMany(s => s.Branches)
            .HasForeignKey(s => s.CompanyId);

    }
}