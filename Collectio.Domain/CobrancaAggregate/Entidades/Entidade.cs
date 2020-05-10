using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public abstract class Entidade : BaseTenantEntity
    {
        private string _nome;
        private CpfCnpjValueObject _cpfCnpj;
        private Endereco.Endereco _endereco;

        public string Nome => _nome;
        public CpfCnpjValueObject CpfCnpj => _cpfCnpj;
        public Endereco.Endereco Endereco => _endereco;

        public Entidade(string nome, CpfCnpjValueObject cpfCnpj, Endereco.Endereco endereco)
        {
            _nome = nome;
            _cpfCnpj = cpfCnpj;
            _endereco = endereco;
        }
    }
}