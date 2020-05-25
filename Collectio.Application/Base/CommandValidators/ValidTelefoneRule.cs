using System.Text.RegularExpressions;
using FluentValidation;

namespace Collectio.Application.Base.CommandValidators
{
    public static class ValidTelefoneRule
    {
        public static IRuleBuilderOptions<T, string> IsValidDddTelefone<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder
                .Length(2)
                .WithName("DDD")
                .Must(e => Regex.IsMatch(e, @"(^\d*$)"))
                .WithMessage("DDD informádo inválido");

        public static IRuleBuilderOptions<T, string> IsValidNumeroTelefone<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder
                .MinimumLength(8).MaximumLength(9)
                .WithName("Número do telefone")
                .Must(e => Regex.IsMatch(e, @"(^\d*$)"))
                .WithMessage("Telefone informádo inválido");
    }
}