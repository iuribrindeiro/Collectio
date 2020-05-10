using System;
using Collectio.Domain.Base.ValueObjects;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public class Emissor : Entidade
    {
        public Emissor(string nome, CpfCnpjValueObject cpfCnpj, Endereco.Endereco endereco) : base(nome, cpfCnpj, endereco)
        {
        }

        protected override IValidator ValidatorFactory()
        {
            throw new NotImplementedException();
        }
    }
}