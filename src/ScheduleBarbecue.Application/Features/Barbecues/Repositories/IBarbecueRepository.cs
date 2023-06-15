using ScheduleBarbecue.Application.Features.Barbecues.DTOs;

namespace ScheduleBarbecue.Application.Features.Barbecues.Repositories;

public interface IBarbecueRepository
{
    Task AddAsync(Barbecue barbecue);

    Task<Barbecue?> GetByIdAsync(Guid id);

    Task<List<Barbecue>> GetAllAsync();

    IQueryable<BarbecueDto> GetAllOrOneCompendiousAsync(Guid? id);
}