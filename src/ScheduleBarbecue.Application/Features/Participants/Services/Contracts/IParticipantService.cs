using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Features.Participants.Requests;
using ScheduleBarbecue.Application.Features.Participants.Responses;

namespace ScheduleBarbecue.Application.Features.Participants.Services.Contracts;

public interface IParticipantService
{
    Task<(Response, Participant?)> CreateParticipant(CreateParticipantRequest request, CancellationToken cancellationToken = default);

    Task<(Response, List<ParticipantResponse?>)> GetAllParticipant(CancellationToken cancellationToken = default);
}