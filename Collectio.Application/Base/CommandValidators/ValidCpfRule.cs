﻿using FluentValidation;

namespace Collectio.Application.Base.CommandValidators
{
    public static class ValidCpfRule
    {
        public static IRuleBuilderOptions<T, string> IsValidCpf<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.Must(e => e.IsCpf()).WithMessage("CPF inválido");
    }
}