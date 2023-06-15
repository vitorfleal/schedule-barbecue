using ScheduleBarbecue.Application.Features.Barbecues;
using ScheduleBarbecue.Application.Features.Barbecues.DTOs;
using ScheduleBarbecue.Application.Features.Barbecues.Requests;
using ScheduleBarbecue.Application.Features.Barbecues.Responses;

namespace ScheduleBarbecue.Tests.Helpers.Features;

public static class BarbecueHelper
{
    public static Barbecue NewBarbecue() =>
        new("Teste Barbecue", "Descrição Barbecue", "Informações Adicionais Barbecue", DateTime.UtcNow, 20m, 10m, true);

    public static CreateBarbecueRequest NewCreateBarbecueRequest() =>
        new()
        {
            Name = "Teste Barbecue",
            Description = "Descrição Barbecue",
            AdditionalNote = "Informações Adicionais Barbecue",
            Date = DateTime.UtcNow,
            SuggestedValueWithDrink = 20m,
            SuggestedValueWithoutDrink = 10m,
            HasSuggestedValue = true,
        };

    public static List<Barbecue> ListBarbecue() =>
        new()
        {
            new("Teste Barbecue 1", "Descrição Barbecue 2", "Informações Adicionais Barbecue 1", DateTime.UtcNow, 20m, 10m, true),
            new("Teste Barbecue 2", "Descrição Barbecue 2", "Informações Adicionais Barbecue 2", DateTime.UtcNow, 30m, 20m, true),
        };

    public static List<BarbecueResponse?> ListBarbecueResponse() =>
        new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Teste Barbecue 1",
                Description = "Descrição Barbecue 1",
                AdditionalNote = "Informações Adicionais Barbecue 1",
                Date = DateTime.UtcNow,
                SuggestedValueWithDrink = 20m,
                SuggestedValueWithoutDrink = 10m,
                HasSuggestedValue = true,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Teste Barbecue 2",
                Description = "Descrição Barbecue 2",
                AdditionalNote = "Informações Adicionais Barbecue 2",
                Date = DateTime.UtcNow,
                SuggestedValueWithDrink = 30m,
                SuggestedValueWithoutDrink = 20m,
                HasSuggestedValue = true,
            },
        };

    public static List<BarbecueDto> ListBarbecueDto() =>
        new()
        {
            new()
            {
                Id = new Guid("7cffd210-74b5-4784-b6fd-05af942cf582"),
                Name = "Teste Barbecue 1",
                Date = DateTime.UtcNow,
                TotalParticipants = 2,
                ContributionAmount = 100m,
            },
            new()
            {
                Id = new Guid("d5ccd007-cf1d-4e84-9f15-051b1936627d"),
                Name = "Teste Barbecue 2",
                Date = DateTime.UtcNow,
                TotalParticipants = 3,
                ContributionAmount = 200m,
            },
        };

    public static List<BarbecueCompendiousResponse?> ListBarbecueCompendiousResponse() =>
        new()
        {
            new()
            {
                Id = new Guid("7cffd210-74b5-4784-b6fd-05af942cf582"),
                Name = "Teste Barbecue 1",
                Date = DateTime.UtcNow,
                TotalParticipants = 2,
                ContributionAmount = 100m,
            },
            new()
            {
                Id = new Guid("d5ccd007-cf1d-4e84-9f15-051b1936627d"),
                Name = "Teste Barbecue 2",
                Date = DateTime.UtcNow,
                TotalParticipants = 3,
                ContributionAmount = 200m,
            },
        };
}