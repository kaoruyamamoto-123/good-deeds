using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class OrganizationCategoriesConfiguration : IEntityTypeConfiguration<OrganizationCategory>
{
    public void Configure(EntityTypeBuilder<OrganizationCategory> builder)
    {
        builder.HasKey(oc => new { oc.OrganizationId, oc.CategoryId });
        
        builder.HasOne(oc => oc.Organization)
            .WithMany(o => o.Categories)
            .HasForeignKey(oc => oc.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(oc => oc.Category)
            .WithMany()
            .HasForeignKey(oc => oc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}