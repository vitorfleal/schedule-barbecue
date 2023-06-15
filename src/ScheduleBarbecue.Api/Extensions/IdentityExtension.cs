using Microsoft.AspNetCore.Identity;
using ScheduleBarbecue.Application.Features.Users;
using ScheduleBarbecue.Infrastructure.Contexts;

namespace ScheduleBarbecue.Api.Extensions;

public static class IdentityExtension
{
    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(config =>
        {
            // Password settings
            config.Password.RequireDigit = false;
            config.Password.RequiredLength = 4;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
            config.Password.RequireLowercase = false;
            config.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ScheduleBarbecueContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}