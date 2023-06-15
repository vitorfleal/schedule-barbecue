using Microsoft.EntityFrameworkCore;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Features.Barbecues.Repositories;
using ScheduleBarbecue.Application.Features.Contributions.Repositories;
using ScheduleBarbecue.Application.Features.Participants.Repositories;
using ScheduleBarbecue.Infrastructure.Contexts;
using ScheduleBarbecue.Infrastructure.Contexts.Persistence;
using ScheduleBarbecue.Infrastructure.Repositories;

namespace ScheduleBarbecue.Api.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        // contexts

        if (!env.IsEnvironment("Testing"))
        {
            services.AddDbContext<ScheduleBarbecueContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"), builder =>
                    builder.MigrationsAssembly("ScheduleBarbecue.Infrastructure"))
                );
        }

        // add unit of work

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // repositories

        services.AddTransient<IBarbecueRepository, BarbecueRepository>();
        services.AddTransient<IContributionRepository, ContributionRepository>();
        services.AddTransient<IParticipantRepository, ParticipantRepository>();

        return services;
    }
}