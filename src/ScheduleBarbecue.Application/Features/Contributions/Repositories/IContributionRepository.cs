using ScheduleBarbecue.Application.Features.Contributions.DTOs;

namespace ScheduleBarbecue.Application.Features.Contributions.Repositories;

public interface IContributionRepository
{
    Task AddAsync(Contribution contribution);

    void Remove(Contribution contribution);

    Task<Contribution?> GetByIdAsync(Guid id);

    Task<List<ContributionParticipantDto>> GetAllParticipantByBarbecueIdAsync(Guid barbecueId);
}