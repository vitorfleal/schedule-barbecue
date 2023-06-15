using FluentValidation;
using FluentValidation.AspNetCore;
using ScheduleBarbecue.Application.Features.Barbecues.Validators;
using ScheduleBarbecue.Application.Features.Contributions.Validators;
using ScheduleBarbecue.Application.Features.Participants.Validators;
using ScheduleBarbecue.Application.Features.Users.Validators;

namespace ScheduleBarbecue.Api.Extensions;

public static class ValidationsExtension
{
    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddValidatorsFromAssemblyContaining<CreaateBarbecueValidator>();

        services.AddValidatorsFromAssemblyContaining<CreateContributionValidator>();

        services.AddValidatorsFromAssemblyContaining<CreateParticipantValidator>();

        services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

        services.AddValidatorsFromAssemblyContaining<SendUserLoginValidator>();

        return services;
    }
}