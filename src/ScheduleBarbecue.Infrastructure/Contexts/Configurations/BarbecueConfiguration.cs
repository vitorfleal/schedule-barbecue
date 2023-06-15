using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleBarbecue.Application.Features.Barbecues;

namespace ScheduleBarbecue.Infrastructure.Contexts.Configurations;

public class BarbecueConfiguration : IEntityTypeConfiguration<Barbecue>
{
    public void Configure(EntityTypeBuilder<Barbecue> builder)
    {
        builder.ToTable(nameof(Barbecue));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description).IsRequired(false);
        builder.Property(x => x.AdditionalNote).IsRequired(false);
        builder.Property(x => x.SuggestedValueWithDrink).HasColumnType("decimal(15,2)").IsRequired();
        builder.Property(x => x.SuggestedValueWithoutDrink).HasColumnType("decimal(15,2)").IsRequired();
        builder.Property(x => x.HasSuggestedValue).IsRequired();
    }
}