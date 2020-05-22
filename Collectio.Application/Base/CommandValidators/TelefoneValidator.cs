using System.Text.RegularExpressions;
using FluentValidation;

namespace Collectio.Application.Base.CommandValidators
{
    public static class TelefoneValidator
    {
        public static IRuleBuilderOptions<T, string> IsValidTelefone<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.NotEmpty()
                .MinimumLength(8).MaximumLength(9)
                .Must(e => Regex.IsMatch(e, @"^\d$"))
                .WithMessage("O número de telefone deve conter de 8 a 9 dígitos numéricos");
    }
}