namespace ScheduleBarbecue.Application.Features.Participants.Repositories;

public interface IParticipantRepository
{
    Task AddAsync(Participant participant);

    Task<Participant?> GetByIdAsync(Guid id);

    Task<List<Participant>> GetAllAsync();
}