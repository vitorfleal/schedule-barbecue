namespace ScheduleBarbecue.Application.Features.Contributions.DTOs;

public class ContributionParticipantDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Value { get; set; }
}