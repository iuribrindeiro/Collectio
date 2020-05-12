using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using System;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public class Emissor : BaseTenantEntity, IEntidade
    {
        private string _nome;
        private CpfCnpjValueObject _cpfCnpj;
        private Endereco _endereco;

        public Emissor(string nome, CpfCnpjValueObject cpfCnpj, Endereco endereco)
        {
            _nome = nome;
            _cpfCnpj = cpfCnpj;
            _endereco = endereco;
        }

        public Emissor(Guid id, string nome, CpfCnpjValueObject cpfCnpj, Endereco endereco) : base(id)
        {
            _nome = nome;
            _cpfCnpj = cpfCnpj;
            _endereco = endereco;
        }

        public string Nome => _nome;
        public CpfCnpjValueObject CpfCnpj => _cpfCnpj;
        public Endereco Endereco => _endereco;
    }
}