namespace ScheduleBarbecue.Application.Features.Contributions.Requests;

public class CreateContributionRequest
{
    public Guid BarbecueId { get; set; }
    public Guid ParticipantId { get; set; }
    public decimal Value { get; set; }
    public bool WithDrink { get; set; }
}