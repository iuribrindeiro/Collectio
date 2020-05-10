using Collectio.Domain.Base.ValueObjects;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public class Pagador : Entidade
    {
        public Pagador(string nome, CpfCnpjValueObject cpfCnpj, Endereco.Endereco endereco) : base(nome, cpfCnpj, endereco)
        {
        }

        protected override IValidator ValidatorFactory()
            => new EntidadeValidator<Pagador>();
    }
}