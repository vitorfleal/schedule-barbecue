using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Features.Barbecues.Requests;
using ScheduleBarbecue.Application.Features.Barbecues.Responses;

namespace ScheduleBarbecue.Application.Features.Barbecues.Services.Contracts;

public interface IBarbecueService
{
    Task<(Response, Barbecue?)> CreateBarbecue(CreateBarbecueRequest request, CancellationToken cancellationToken = default);

    Task<(Response, List<BarbecueResponse?>)> GetAllBarbecue(CancellationToken cancellationToken = default);

    Task<(Response, List<BarbecueCompendiousResponse>)> GetAllOrOneBarbecueCompendious(Guid? id = default, CancellationToken cancellationToken = default);
}