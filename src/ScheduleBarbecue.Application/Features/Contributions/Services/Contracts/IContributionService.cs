using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Features.Contributions.Requests;
using ScheduleBarbecue.Application.Features.Contributions.Responses;

namespace ScheduleBarbecue.Application.Features.Contributions.Services.Contracts;

public interface IContributionService
{
    Task<(Response, Contribution?)> CreateContribution(CreateContributionRequest request, CancellationToken cancellationToken = default);

    Task<Response> RemoveContribution(Guid id, CancellationToken cancellationToken = default);

    Task<(Response, List<ContributionParticipantResponse>)> GetAllParticipantByBarbecueId(Guid barbecueId, CancellationToken cancellationToken = default);
}