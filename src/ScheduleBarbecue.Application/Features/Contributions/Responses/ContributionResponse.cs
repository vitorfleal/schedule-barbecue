namespace ScheduleBarbecue.Application.Features.Contributions.Responses;

public class ContributionResponse
{
    public Guid Id { get; set; }
    public Guid BarbecueId { get; set; }
    public Guid ParticipantId { get; set; }
    public decimal Value { get; set; }
}