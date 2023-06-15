using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Features.Barbecues;
using ScheduleBarbecue.Application.Features.Participants;

namespace ScheduleBarbecue.Application.Features.Contributions;

public class Contribution : Entity
{
    public Contribution(Guid barbecueId, Guid participantId, decimal value)
    {
        BarbecueId = barbecueId;
        ParticipantId = participantId;
        Value = value;
    }

    public Contribution()
    {
    }

    public Guid BarbecueId { get; private set; }
    public Guid ParticipantId { get; private set; }
    public decimal Value { get; private set; }

    public Barbecue? Barbecue { get; set; }
    public Participant? Participant { get; set; }
}