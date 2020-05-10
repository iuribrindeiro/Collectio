using System.Collections.Generic;
using Collectio.Domain.Base.ValueObjects;
using FluentValidation;

namespace Collectio.Domain.Base.Validators
{
    public static class CpfValidator
    {
        public static IRuleBuilderOptions<T, R> IsValid<T, R>(this IRuleBuilder<T, R> ruleBuilder) where R : CpfValueObject 
            => ruleBuilder.Must(e => e.Value.IsCpf()).WithMessage("CPF inválido");
    }
}
