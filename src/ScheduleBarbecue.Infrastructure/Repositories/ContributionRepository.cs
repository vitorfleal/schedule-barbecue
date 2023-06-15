using Microsoft.EntityFrameworkCore;
using ScheduleBarbecue.Application.Features.Contributions;
using ScheduleBarbecue.Application.Features.Contributions.DTOs;
using ScheduleBarbecue.Application.Features.Contributions.Repositories;
using ScheduleBarbecue.Application.Features.Participants;
using ScheduleBarbecue.Infrastructure.Contexts;

namespace ScheduleBarbecue.Infrastructure.Repositories;

public class ContributionRepository : IContributionRepository
{
    private readonly DbSet<Contribution> _contribution;
    private readonly DbSet<Participant> _participant;

    public ContributionRepository(ScheduleBarbecueContext context)
    {
        _contribution = context.Contributions;
        _participant = context.Participants;
    }

    public async Task AddAsync(Contribution contribution)
    {
        await _contribution.AddAsync(contribution);
    }

    public async Task<Contribution?> GetByIdAsync(Guid id) => await _contribution.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public void Remove(Contribution contribution)
    {
        _contribution.Remove(contribution);
    }

    public async Task<List<ContributionParticipantDto>> GetAllParticipantByBarbecueIdAsync(Guid barbecueId)
    => await _participant.AsNoTracking()
        .Join(_contribution.AsNoTracking(),
        pt => pt.Id,
        cb => cb.ParticipantId,
        (participant, contribution) => new { participant, contribution })
        .Where(x => x.contribution.BarbecueId == barbecueId)
        .Select(s => new ContributionParticipantDto
        {
            Id = s.contribution.Id,
            Name = s.participant.Name,
            Value = s.contribution.Value
        })
        .ToListAsync();
}