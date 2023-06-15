using Microsoft.EntityFrameworkCore;
using ScheduleBarbecue.Application.Features.Contributions;
using ScheduleBarbecue.Application.Features.Participants;
using ScheduleBarbecue.Application.Features.Participants.Repositories;
using ScheduleBarbecue.Infrastructure.Contexts;

namespace ScheduleBarbecue.Infrastructure.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly DbSet<Participant> _participant;
    private readonly DbSet<Contribution> _contribution;

    public ParticipantRepository(ScheduleBarbecueContext context)
    {
        _participant = context.Participants;
        _contribution = context.Contributions;
    }

    public async Task AddAsync(Participant participant)
    {
        await _participant.AddAsync(participant);
    }

    public async Task<Participant?> GetByIdAsync(Guid id) => await _participant.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<Participant>> GetAllAsync() => await _participant.AsNoTracking().ToListAsync();
}