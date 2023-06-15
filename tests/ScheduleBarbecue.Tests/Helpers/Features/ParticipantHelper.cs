using ScheduleBarbecue.Application.Features.Participants;
using ScheduleBarbecue.Application.Features.Participants.Requests;
using ScheduleBarbecue.Application.Features.Participants.Responses;

namespace ScheduleBarbecue.Tests.Helpers.Features;

public class ParticipantHelper
{
    public static Participant NewParticipant() =>
        new("Teste Participante");

    public static CreateParticipantRequest NewCreateParticipantRequest() =>
        new()
        {
            Name = "Teste Participante",
        };

    public static List<Participant> ListParticipant() =>
        new()
        {
            new("Teste Participante 1"),
            new("Teste Participante 2"),
        };

    public static List<ParticipantResponse> ListParticipantResponse() =>
        new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Teste Participante 1",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Teste Participante 2",
            },
        };
}