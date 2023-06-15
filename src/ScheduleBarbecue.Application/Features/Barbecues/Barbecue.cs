using ScheduleBarbecue.Application.Base.Models;
using ScheduleBarbecue.Application.Features.Contributions;

namespace ScheduleBarbecue.Application.Features.Barbecues;

public class Barbecue : Entity
{
    public Barbecue(
        string? name,
        string? description,
        string? additionalNote,
        DateTime? date,
        decimal suggestValueWithDrink,
        decimal suggestValueWithoutDrink,
        bool hasSuggestedValue)
    {
        Name = name;
        Description = description;
        AdditionalNote = additionalNote;
        Date = date;
        SuggestedValueWithDrink = suggestValueWithDrink;
        SuggestedValueWithoutDrink = suggestValueWithoutDrink;
        HasSuggestedValue = hasSuggestedValue;
    }

    public Barbecue()
    {
    }

    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public string? AdditionalNote { get; private set; }
    public DateTime? Date { get; private set; }
    public decimal SuggestedValueWithDrink { get; private set; }
    public decimal SuggestedValueWithoutDrink { get; private set; }
    public bool HasSuggestedValue { get; private set; }

    public List<Contribution>? Contribution { get; set; }
}