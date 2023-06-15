using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Features.Contributions;

namespace ScheduleBarbecue.Application.Features.Participants;

public class Participant : Entity
{
    public Participant(string? name)
    {
        Name = name;
    }

    public Participant()
    {
    }

    public string? Name { get; private set; }

    public List<Contribution>? Contribution { get; set; }
}