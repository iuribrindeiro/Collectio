using FluentValidation;

namespace Collectio.Application.Base.CommandValidators
{
    public static class ValidAgenciaRule
    {
        public static IRuleBuilderOptions<T, string> IsValidAgenciaBancaria<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.MaximumLength(6);
    }
}