using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleBarbecue.Application.Features.Contributions;

namespace ScheduleBarbecue.Infrastructure.Contexts.Configurations;

public class ContributionConfiguration : IEntityTypeConfiguration<Contribution>
{
    public void Configure(EntityTypeBuilder<Contribution> builder)
    {
        builder.ToTable(nameof(Contribution));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Value).HasColumnType("decimal(15,2)").IsRequired();

        builder.HasOne(x => x.Barbecue)
                 .WithMany(x => x.Contribution)
                 .HasForeignKey(x => x.BarbecueId)
                 .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Participant)
                 .WithMany(x => x.Contribution)
                 .HasForeignKey(x => x.ParticipantId)
                 .OnDelete(DeleteBehavior.ClientCascade);
    }
}