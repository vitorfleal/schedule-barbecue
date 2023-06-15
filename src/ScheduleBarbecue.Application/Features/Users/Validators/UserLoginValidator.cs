using FluentValidation;
using ScheduleBarbecue.Application.Features.Users.Requests;
using System.Linq.Expressions;

namespace ScheduleBarbecue.Application.Features.Users.Validators;

public class UserLoginValidator<T> : AbstractValidator<T> where T : UserLoginRequest
{
    public UserLoginValidator()
    {
        RuleRequiredFor(userLoginRequest => userLoginRequest.UserName, "Usuário");
        RuleRequiredFor(userLoginRequest => userLoginRequest.Password, "Senha");
    }

    public void RuleRequiredFor<TProperty>(Expression<Func<T, TProperty>> expression, string label)
    {
        RuleFor(expression)
            .NotEmpty().WithMessage($"{label} do usuário é obrigatório.");
    }
}
