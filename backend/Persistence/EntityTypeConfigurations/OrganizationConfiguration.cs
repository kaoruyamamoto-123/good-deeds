using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Domain.Models;

namespace Persistence.EntityTypeConfigurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedNever();
        
        builder.Property(o => o.Title)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.HasIndex(o => o.Title)
            .IsUnique(false)
            .HasMethod("gin");
        
        builder.Property(o => o.Description)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(o => o.Phone).HasMaxLength(255);

        builder.OwnsOne(o => o.Address, navigationBuilder =>
        {
            navigationBuilder.Property(a => a.City).HasMaxLength(255);
            navigationBuilder.Property(a => a.Street).HasMaxLength(255);
        });
        
        builder.OwnsOne(o => o.Coordinates, navigationBuilder =>
        {
            navigationBuilder.Property(o => o.Latitude).HasColumnName("latitude");
            navigationBuilder.Property(o => o.Longitude).HasColumnName("longitude");
        });
        
        builder.Property(o => o.Link)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(o => o.LogoPath).HasMaxLength(255);

        builder.Property(o => o.CreatedAt).IsRequired();
    }
}