using System;
using Collectio.Domain.ClienteAggregate;
using FluentValidation;

namespace Collectio.Application.Clientes.CommandValidators
{
    public static class ClienteExisteValidator
    {
        public static IRuleBuilderOptions<T, string> ExisteClienteComId<T>(this IRuleBuilder<T, string> ruleBuilder, IClientesRepository clientesRepository)
        {
            return ruleBuilder.MustAsync(async (id, b) =>
            {
                if (!Guid.TryParse(id, out Guid clienteId))
                    return false;

                return await clientesRepository.ExistsAsync(clienteId);
            }).WithMessage("O cliente informado não existe");
        }
    }
}
