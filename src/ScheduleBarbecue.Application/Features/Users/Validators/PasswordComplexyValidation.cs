using FluentValidation;
using System.Text.RegularExpressions;

namespace ScheduleBarbecue.Application.Features.Users.Validators;

public class PasswordComplexyValidation : AbstractValidator<string>
{
    private static readonly Regex SequenceCharactersRegex = CreateCompiledRegex(
        "abc|bcd|cde|def|efg|fgh|ghi|hij|ijk|jkl|klm|lmn|mno|nop|opq|pqr|qrs|rst|stu|tuv|uvw|vwx|wxy|xyz|012|123|234|345|456|567|678|789");

    private static readonly Regex AtLeastWhiteSpaceRegex = CreateCompiledRegex(@"\A\S{3,15}\z");

    public PasswordComplexyValidation()
    {
        RuleFor(x => x).Must(AtLeastWhiteSpaceRegex.IsMatch)
            .When(x => !string.IsNullOrEmpty(x))
            .WithMessage("A senha não deve ter conter espaços em branco.");

        RuleFor(x => x).MinimumLength(4)
            .When(x => !string.IsNullOrEmpty(x))
            .WithMessage("A senha deve ter no mínimo 4 caracteres.");

        RuleFor(x => x).MaximumLength(64)
            .WithMessage("A senha pode ter no máximo 64 caracteres.");

        RuleFor(x => x).Must(x => !SequenceCharactersRegex.IsMatch(x))
            .WithMessage("A senha não pode conter sequência de caracteres (abc, 123, etc).");
    }

    private static Regex CreateCompiledRegex(string pattern) =>
        new(
            pattern,
            RegexOptions.IgnoreCase | RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(500));
}