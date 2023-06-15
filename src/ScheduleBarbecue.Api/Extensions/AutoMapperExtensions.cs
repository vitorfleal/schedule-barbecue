using ScheduleBarbecue.Application.AutoMapper;

namespace ScheduleBarbecue.Api.Extensions;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ConfiguringMapperProfile));

        return services;
    }
}