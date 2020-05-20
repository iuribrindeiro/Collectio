using System.Text.RegularExpressions;
using FluentValidation;

namespace Collectio.Application.Base.CommandValidators
{
    public static class DddValidator
    {
        public static IRuleBuilderOptions<T, string> IsValidDdd<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.NotEmpty()
                .Length(2)
                .Must(e => Regex.IsMatch(e, @"^\d$"))
                .WithMessage("DDD inválido");
    }
}