using FluentValidation;
using ScheduleBarbecue.Application.Features.Barbecues.Requests;
using System.Linq.Expressions;

namespace ScheduleBarbecue.Application.Features.Barbecues.Validators;

public class BarbecueValidator<T> : AbstractValidator<T> where T : CreateBarbecueRequest
{
    public BarbecueValidator()
    {
        RuleRequiredFor(barbecueRequest => barbecueRequest.Name, "Nome");
        RuleRequiredFor(barbecueRequest => barbecueRequest.Date, "Data");

        RuleFor(barbecueRequest => barbecueRequest.Date)
                    .Must(barbecueDate => barbecueDate >= DateTime.Today)
                    .WithMessage("A data do churrasco não pode ser menor do que a data atual.");

        When(barbecueRequest => barbecueRequest.HasSuggestedValue, () =>
        {
            RuleFor(barbecueRequest => barbecueRequest)
            .Must(barbecueRequest => barbecueRequest.SuggestedValueWithDrink >= decimal.One || barbecueRequest.SuggestedValueWithoutDrink >= decimal.One)
            .WithMessage("Ao menos um valor sugerido com ou sem bebida deve ser informado.");
        });
    }

    public void RuleRequiredFor<TProperty>(Expression<Func<T, TProperty>> expression, string label)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage($"{label} do churrasco é obrigatório.");
    }
}