using FluentValidation;
using ScheduleBarbecue.Application.Features.Users.Requests;
using System.Linq.Expressions;

namespace ScheduleBarbecue.Application.Features.Users.Validators;

public class UserValidator<T> : AbstractValidator<T> where T : CreateUserRequest
{
    public UserValidator()
    {
        RuleRequiredFor(userRequest => userRequest.Name, "Nome");
        RuleRequiredFor(userRequest => userRequest.Email, "E-mail");
        RuleRequiredFor(userRequest => userRequest.Password, "Senha");

        RuleFor(userRequest => userRequest.Password)
            .SetValidator(new PasswordComplexyValidation());
        RuleFor(userRequest => userRequest.Email)
            .EmailAddress().WithMessage("E-mail informado inválido.");
    }

    public void RuleRequiredFor<TProperty>(Expression<Func<T, TProperty>> expression, string label)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage($"{label} do usuário é obrigatório.");
    }
}