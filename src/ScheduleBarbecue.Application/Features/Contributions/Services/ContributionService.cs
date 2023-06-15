using AutoMapper;
using Microsoft.AspNetCore.Http;
using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Base.Services;
using ScheduleBarbecue.Application.Features.Barbecues.Repositories;
using ScheduleBarbecue.Application.Features.Contributions.Repositories;
using ScheduleBarbecue.Application.Features.Contributions.Requests;
using ScheduleBarbecue.Application.Features.Contributions.Responses;
using ScheduleBarbecue.Application.Features.Contributions.Services.Contracts;
using ScheduleBarbecue.Application.Features.Participants.Repositories;

namespace ScheduleBarbecue.Application.Features.Contributions.Services;

public class ContributionService : AppService, IContributionService
{
    private readonly IMapper _mapper;
    private readonly IContributionRepository _contributionRepository;
    private readonly IBarbecueRepository _barbecueRepository;
    private readonly IParticipantRepository _participantRepository;

    public ContributionService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IContributionRepository contributionRepository,
        IBarbecueRepository barbecueRepository,
        IParticipantRepository participantRepository
        ) : base(unitOfWork)
    {
        _mapper = mapper;
        _contributionRepository = contributionRepository;
        _barbecueRepository = barbecueRepository;
        _participantRepository = participantRepository;
    }

    public async Task<(Response, Contribution?)> CreateContribution(CreateContributionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var contribution = new Contribution(request.BarbecueId, request.ParticipantId, request.Value);

            var barbecue = await _barbecueRepository.GetByIdAsync(request.BarbecueId);

            if (barbecue is null)
                return (Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), "Churrasco não localizado."), null);

            var participant = await _participantRepository.GetByIdAsync(request.ParticipantId);

            if (participant is null)
                return (Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), "Participante não localizado."), null);

            if (request.WithDrink && barbecue.HasSuggestedValue && request.Value < barbecue.SuggestedValueWithDrink)
                return (Response.Invalid(StatusCodes.Status400BadRequest.ToString(), $"Valor da contribuição com bebida informado deve ser igual ou maior que {string.Format("{0:C}", barbecue.SuggestedValueWithDrink)}."), contribution);

            if (!request.WithDrink && barbecue.HasSuggestedValue && request.Value < barbecue.SuggestedValueWithoutDrink)
                return (Response.Invalid(StatusCodes.Status400BadRequest.ToString(), $"Valor mínimo da contribuição informado deve ser igual ou maior que {string.Format("{0:C}", barbecue.SuggestedValueWithoutDrink)}."), contribution);

            await _contributionRepository.AddAsync(contribution);
            await Commit();

            return (Response.Valid(), contribution);
        }
        catch (Exception ex)
        {
            return (Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), ex.Message), null);
        }
    }

    public async Task<Response> RemoveContribution(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var contribution = await _contributionRepository.GetByIdAsync(id);

            if (contribution is null)
                return Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), "Contribuição não localizada.");

            _contributionRepository.Remove(contribution);

            await Commit();

            return Response.Valid();
        }
        catch (Exception ex)
        {
            return Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), ex.Message);
        }
    }

    public async Task<(Response, List<ContributionParticipantResponse>)> GetAllParticipantByBarbecueId(Guid barbecueId, CancellationToken cancellationToken = default)
    => (Response.Valid(), _mapper.Map<List<ContributionParticipantResponse>>(await _contributionRepository.GetAllParticipantByBarbecueIdAsync(barbecueId)));
}