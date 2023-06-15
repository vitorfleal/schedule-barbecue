using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Base.Persistence;
using ScheduleBarbecue.Application.Base.Services;
using ScheduleBarbecue.Application.Features.Barbecues.Repositories;
using ScheduleBarbecue.Application.Features.Barbecues.Requests;
using ScheduleBarbecue.Application.Features.Barbecues.Responses;
using ScheduleBarbecue.Application.Features.Barbecues.Services.Contracts;

namespace ScheduleBarbecue.Application.Features.Barbecues.Services;

public class BarbecueService : AppService, IBarbecueService
{
    private readonly IMapper _mapper;
    private readonly IBarbecueRepository _barbecueRepository;

    public BarbecueService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IBarbecueRepository barbecueRepository
        ) : base(unitOfWork)
    {
        _mapper = mapper;
        _barbecueRepository = barbecueRepository;
    }

    public async Task<(Response, Barbecue?)> CreateBarbecue(CreateBarbecueRequest request, CancellationToken cancellationToken = default)
    {
        var barbecue = new Barbecue
            (
            request.Name,
            request.Description,
            request.AdditionalNote,
            request.Date,
            request.SuggestedValueWithDrink,
            request.SuggestedValueWithoutDrink,
            request.HasSuggestedValue
            );

        try
        {
            await _barbecueRepository.AddAsync(barbecue);

            await Commit();

            return (Response.Valid(), barbecue);
        }
        catch (Exception ex)
        {
            return (Response.Invalid(StatusCodes.Status422UnprocessableEntity.ToString(), ex.Message), barbecue);
        }
    }

    public async Task<(Response, List<BarbecueResponse?>)> GetAllBarbecue(CancellationToken cancellationToken = default)
        => (Response.Valid(), _mapper.Map<List<BarbecueResponse?>>(await _barbecueRepository.GetAllAsync()));

    public async Task<(Response, List<BarbecueCompendiousResponse>)> GetAllOrOneBarbecueCompendious(Guid? id = default, CancellationToken cancellationToken = default)
    => (Response.Valid(), _mapper.Map<List<BarbecueCompendiousResponse>>(await _barbecueRepository.GetAllOrOneCompendiousAsync(id).ToListAsync()));
}