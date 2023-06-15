namespace ScheduleBarbecue.Application.Features.Barbecues.DTOs;

public class BarbecueDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime? Date { get; set; }
    public int? TotalParticipants { get; set; }
    public decimal? ContributionAmount { get; set; }
}