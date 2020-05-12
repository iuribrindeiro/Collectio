using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using System;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public class Pagador : BaseTenantEntity, IEntidade
    {
        private Endereco _endereco;
        private CpfCnpjValueObject _cpfCnpj;
        private string _nome;

        public Pagador(string nome, CpfCnpjValueObject cpfCnpj, Endereco endereco)
        {
            _nome = nome;
            _cpfCnpj = cpfCnpj;
            _endereco = endereco;
        }

        public Pagador(Guid id, string nome, CpfCnpjValueObject cpfCnpj, Endereco endereco) : base(id)
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