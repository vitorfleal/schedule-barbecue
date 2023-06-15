namespace ScheduleBarbecue.Application.Features.Barbecues.Responses;

public class BarbecueResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? AdditionalNote { get; set; }
    public DateTime? Date { get; set; }
    public decimal SuggestedValueWithDrink { get; set; }
    public decimal SuggestedValueWithoutDrink { get; set; }
    public bool HasSuggestedValue { get; set; }
}