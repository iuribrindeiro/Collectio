﻿using FluentValidation;

namespace Collectio.Application.Base.CommandValidators
{
    public static class CnpjValidator
    {
        public static IRuleBuilderOptions<T, string> IsValidCnpj<T>(this IRuleBuilder<T, string> ruleBuilder)
            => ruleBuilder.Must(e => e.IsCnpj()).WithMessage("CNPJ inválido");
    }
}