using FluentValidation;

namespace Collectio.Application.Base.CommandValidators
{
    public static class ValidContaRule
    {
        public static IRuleBuilderOptions<T, string> IsValidContaBancaria<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.MaximumLength(20);
    }
}