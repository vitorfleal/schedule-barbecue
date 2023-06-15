using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScheduleBarbecue.Application.Features.Barbecues;
using ScheduleBarbecue.Application.Features.Contributions;
using ScheduleBarbecue.Application.Features.Participants;
using ScheduleBarbecue.Application.Features.Users;
using System.Reflection;

namespace ScheduleBarbecue.Infrastructure.Contexts;

public class ScheduleBarbecueContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ScheduleBarbecueContext(DbContextOptions<ScheduleBarbecueContext> options) : base(options)
    {
    }

    public DbSet<Barbecue> Barbecues => Set<Barbecue>();
    public DbSet<Contribution> Contributions => Set<Contribution>();
    public DbSet<Participant> Participants => Set<Participant>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("User");
        builder.Entity<IdentityRole<Guid>>().ToTable("Role");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken");

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}