using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        
        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasData(
            Category.Create((int)CategoryEnum.AnimalsProtection,"Защита животных"),
            Category.Create((int)CategoryEnum.HealthAndSport, "Здоровье и спорт"),
            Category.Create((int)CategoryEnum.LocalCommunity, "Местное сообщество и развитие территорий"),
            Category.Create((int)CategoryEnum.SocialProtection, "Социальная защита (помощь людям в трудной ситуации)"),
            Category.Create((int)CategoryEnum.Ecology, "Экология и устойчивое развитие"),
            Category.Create((int)CategoryEnum.CultureAndEducation, "Культура и образование"),
            Category.Create((int)CategoryEnum.Other, "Другое")
        );
    }
}