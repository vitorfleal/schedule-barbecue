using FluentValidation;
using ScheduleBarbecue.Application.Features.Participants.Requests;
using System.Linq.Expressions;

namespace ScheduleBarbecue.Application.Features.Participants.Validators;

public class ParticipantValidator<T> : AbstractValidator<T> where T : CreateParticipantRequest
{
    public ParticipantValidator()
    {
        RuleRequiredFor(participantRequest => participantRequest.Name, "Nome");
    }

    public void RuleRequiredFor<TProperty>(Expression<Func<T, TProperty>> expression, string label)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage($"{label} do participante é obrigatório.");
    }
}