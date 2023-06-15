using FluentValidation;
using ScheduleBarbecue.Application.Features.Contributions.Requests;
using System.Linq.Expressions;

namespace ScheduleBarbecue.Application.Features.Contributions.Validators;

public class ContributionValidator<T> : AbstractValidator<T> where T : CreateContributionRequest
{
    public ContributionValidator()
    {
        RuleRequiredFor(contribution => contribution.BarbecueId, "Churrasco Id");
        RuleRequiredFor(contribution => contribution.ParticipantId, "Participante Id");
        RuleRequiredFor(contribution => contribution.Value, "Valor");

        RuleFor(contribution => contribution.Value)
        .GreaterThanOrEqualTo(decimal.Zero)
            .WithMessage("O valor da contribuição deve ser maior que zero.");
    }

    public void RuleRequiredFor<TProperty>(Expression<Func<T, TProperty>> expression, string label)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage($"{label} da contribuição é obrigatório.");
    }
}