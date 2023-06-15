namespace ScheduleBarbecue.Application.Features.Barbecues.Requests;

public class CreateBarbecueRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? AdditionalNote { get; set; }
    public DateTime Date { get; set; }
    public decimal SuggestedValueWithDrink { get; set; }
    public decimal SuggestedValueWithoutDrink { get; set; }
    public bool HasSuggestedValue { get; set; }
}