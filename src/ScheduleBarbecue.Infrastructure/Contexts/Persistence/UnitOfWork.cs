using ScheduleBarbecue.Application.Base.Persistence;

namespace ScheduleBarbecue.Infrastructure.Contexts.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ScheduleBarbecueContext _context;

    public UnitOfWork(ScheduleBarbecueContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}