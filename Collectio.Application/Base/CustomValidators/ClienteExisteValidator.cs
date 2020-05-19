using System;
using Collectio.Domain.ClienteAggregate;
using FluentValidation;

namespace Collectio.Application.Base.CustomValidators
{
    public static class ClienteExisteValidator
    {
        public static IRuleBuilderOptions<T, string> ExisteClienteComId<T>(this IRuleBuilder<T, string> ruleBuilder, IClientesRepository clientesRepository)
        {
            return ruleBuilder.MustAsync(async (id, b) =>
            {
                if (!Guid.TryParse(id, out Guid clienteId))
                    return false;

                return await clientesRepository.Exists(Guid.Parse(id));
            }).WithMessage("O cliente informado não existe");
        }

        public static IRuleBuilderOptions<T, string> ExisteContaBancariaComId<T>(this IRuleBuilder<T, string> ruleBuilder, IClientesRepository clientesRepository)
        {
            return ruleBuilder.MustAsync(async (id, b) =>
            {
                if (!Guid.TryParse(id, out Guid clienteId))
                    return false;

                return await clientesRepository.Exists(Guid.Parse(id));
            }).WithMessage("O cliente informado não existe");
        }
    }
}
