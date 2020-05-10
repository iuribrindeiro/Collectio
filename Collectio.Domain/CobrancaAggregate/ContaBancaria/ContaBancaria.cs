using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.ContaBancaria
{
    public class ContaBancaria : BaseTenantEntity
    {
        private string _nome;
        private ContaBancoValueObject _contaBanco;

        public string Nome => _nome;
        public ContaBancoValueObject ContaBanco => _contaBanco;

        public ContaBancaria(string nome, ContaBancoValueObject contaBanco)
        {
            _nome = nome;
            _contaBanco = contaBanco;
        }

        protected override IValidator ValidatorFactory() 
            => new ContaBancariaValidator();
    }
}