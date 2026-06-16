using BranchService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BranchService.Infrastructura.Persistence.TableConfiguration;

public class CompanyServiceTableConfiguration: IEntityTypeConfiguration<CompanyServiceEntity>
{
    public void Configure(EntityTypeBuilder<CompanyServiceEntity> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(s => s.Id);

        builder.HasOne(s => s.Company)
            .WithMany(s => s.CompanyServices)
            .HasForeignKey(s => s.CompanyId);

    }
}