using Microsoft.EntityFrameworkCore;
using ScheduleBarbecue.Application.Features.Barbecues;
using ScheduleBarbecue.Application.Features.Barbecues.DTOs;
using ScheduleBarbecue.Application.Features.Barbecues.Repositories;
using ScheduleBarbecue.Application.Features.Contributions;
using ScheduleBarbecue.Infrastructure.Contexts;

namespace ScheduleBarbecue.Infrastructure.Repositories;

public class BarbecueRepository : IBarbecueRepository
{
    private readonly DbSet<Barbecue> _barbecue;
    private readonly DbSet<Contribution> _contribution;

    public BarbecueRepository(ScheduleBarbecueContext context)
    {
        _barbecue = context.Barbecues;
        _contribution = context.Contributions;
    }

    public async Task AddAsync(Barbecue barbecue)
    {
        await _barbecue.AddAsync(barbecue);
    }

    public async Task<Barbecue?> GetByIdAsync(Guid id) => await _barbecue.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<Barbecue>> GetAllAsync() => await _barbecue.AsNoTracking().ToListAsync();

    public IQueryable<BarbecueDto> GetAllOrOneCompendiousAsync(Guid? id)
    {
        var query = _barbecue.AsNoTracking()
        .Join(
            _contribution.AsNoTracking(),
            br => br.Id,
            cb => cb.BarbecueId,
            (barbecue, contribution) => new { barbecue, contribution }
            );

        if (id.HasValue && id.Value != Guid.Empty)
        {
            query = query.Where(x => x.barbecue.Id == id);
        }

        return query.GroupBy(b => new { b.barbecue.Id, b.barbecue.Name, b.barbecue.Date })
         .Select(s => new BarbecueDto
         {
             Id = s.Key.Id,
             Name = s.Key.Name,
             Date = s.Key.Date,
             TotalParticipants = s.Count(),
             ContributionAmount = s.Sum(c => c.contribution.Value)
         });
    }
}