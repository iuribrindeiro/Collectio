using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.ContaBancarias
{
    public class ContaBancaria : BaseTenantEntity
    {
        private string _descricao;
        private AgenciaContaValueObject _agenciaConta;
        private string _banco;

        public string Descricao => _descricao;
        public AgenciaContaValueObject AgenciaConta => _agenciaConta;
        public string Banco => _banco;

        public ContaBancaria(string descricao, string banco, AgenciaContaValueObject agenciaConta)
        {
            _descricao = descricao;
            _banco = banco;
            _agenciaConta = agenciaConta;
        }

        protected override IValidator ValidatorFactory() 
            => new ContaBancariaValidator();
    }
}