using ScheduleBarbecue.Application.Features.Contributions;
using ScheduleBarbecue.Application.Features.Contributions.DTOs;
using ScheduleBarbecue.Application.Features.Contributions.Requests;
using ScheduleBarbecue.Application.Features.Contributions.Responses;

namespace ScheduleBarbecue.Tests.Helpers.Features;

public static class ContributionHelper
{
    public static Contribution NewContribution() =>
        new(Guid.NewGuid(), Guid.NewGuid(), 20m);

    public static CreateContributionRequest NewCreateContributionRequest() =>
        new()
        {
            BarbecueId = Guid.NewGuid(),
            ParticipantId = Guid.NewGuid(),
            Value = 20m,
            WithDrink = true,
        };

    public static List<ContributionParticipantDto> ListContributionParticipantDto() =>
        new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Participante 1",
                Value = 20m,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Participante 2",
                Value = 30m,
            },
        };

    public static List<ContributionParticipantResponse?> ListContributionParticipantResponse() =>
        new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Participante 1",
                Value = 20m,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Participante 2",
                Value = 30m,
            },
        };
}