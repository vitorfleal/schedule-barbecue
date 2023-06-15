using ScheduleBarbecue.Application.Features.Barbecues.Services;
using ScheduleBarbecue.Application.Features.Barbecues.Services.Contracts;
using ScheduleBarbecue.Application.Features.Contributions.Services;
using ScheduleBarbecue.Application.Features.Contributions.Services.Contracts;
using ScheduleBarbecue.Application.Features.Participants.Services;
using ScheduleBarbecue.Application.Features.Participants.Services.Contracts;
using ScheduleBarbecue.Application.Features.Tokens.Services;
using ScheduleBarbecue.Application.Features.Tokens.Services.Contracts;
using ScheduleBarbecue.Application.Features.Users.Services;
using ScheduleBarbecue.Application.Features.Users.Services.Contracts;

namespace ScheduleBarbecue.Api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Add handlers, services, repositories

        services.AddTransient<IBarbecueService, BarbecueService>();
        services.AddTransient<IContributionService, ContributionService>();
        services.AddTransient<IParticipantService, ParticipantService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserService, UserService>();

        return services;
    }
}