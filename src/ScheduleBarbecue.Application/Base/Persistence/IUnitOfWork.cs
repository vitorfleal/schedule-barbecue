namespace ScheduleBarbecue.Application.Base.Persistence;

public interface IUnitOfWork
{
    Task Commit();
}