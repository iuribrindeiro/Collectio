using System;
using Collectio.Domain.Base.ValueObjects;
using FluentValidation;

namespace Collectio.Domain.Base.Validators
{
    public static class ContaBancoValidator
    {
        public static IRuleBuilderOptions<T, R> IsValid<T, R>(this IRuleBuilder<T, R> ruleBuilder)
            where R : AgenciaContaValueObject
            => ruleBuilder.Must(e => e.Agencia.Length == 6)
                .WithMessage("Agencia inválida. Deve conter 5 carácteres e 1 digito")
                .Must(e => e.Conta.Length > 1 && e.Conta.Length <= 20)
                .WithMessage("Conta inválida. Deve conter de 1 a 20 carácteres");
    }

    public static class CnpjValidator
    {
        public static IRuleBuilderOptions<T, R> IsValid<T, R>(this IRuleBuilder<T, R> ruleBuilder)
            where R : CpfValueObject
            => ruleBuilder.Must(e => e.Value.IsCnpj()).WithMessage("CNPJ inválido");
    }
}