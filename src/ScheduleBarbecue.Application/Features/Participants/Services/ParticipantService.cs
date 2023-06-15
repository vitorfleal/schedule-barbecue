using AutoMapper;
using Microsoft.AspNetCore.Http;
using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Base.Services;
using ScheduleBarbecue.Application.Features.Participants.Repositories;
using ScheduleBarbecue.Application.Features.Participants.Requests;
using ScheduleBarbecue.Application.Features.Participants.Responses;
using ScheduleBarbecue.Application.Features.Participants.Services.Contracts;

namespace ScheduleBarbecue.Application.Features.Participants.Services;

public class ParticipantService : AppService, IParticipantService
{
    private readonly IMapper _mapper;
    private readonly IParticipantRepository _participantRepository;

    public ParticipantService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IParticipantRepository participantRepository
        ) : base(unitOfWork)
    {
        _mapper = mapper;
        _participantRepository = participantRepository;
    }

    public async Task<(Response, Participant?)> CreateParticipant(CreateParticipantRequest request, CancellationToken cancellationToken = default)
    {
        var participant = new Participant(request.Name);

        try
        {
            await _participantRepository.AddAsync(participant);

            await Commit();

            return (Response.Valid(), participant);
        }
        catch (Exception ex)
        {
            return (Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), ex.Message), participant);
        }
    }

    public async Task<(Response, List<ParticipantResponse?>)> GetAllParticipant(CancellationToken cancellationToken = default)
    => (Response.Valid(), _mapper.Map<List<ParticipantResponse?>>(await _participantRepository.GetAllAsync()));
}