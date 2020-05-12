using Collectio.Domain.Base;
using FluentValidation;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public class Endereco : BaseTenantEntity
    {
        private string _rua;
        private string _numero;
        private string _bairro;
        private string _cep;
        private string _cidade;
        private string _estado;

        public string Rua => _rua;
        public string Numero => _numero;
        public string Bairro => _bairro;
        public string Cep => _cep;
        public string Cidade => _cidade;
        public string Estado => _estado;

        public Endereco(string rua, string numero, string bairro, string cep, string cidade, string estado)
        {
            _rua = rua;
            _numero = numero;
            _bairro = bairro;
            _cep = cep;
            _cidade = cidade;
            _estado = estado;
        }

        protected override IValidator ValidatorFactory() 
            => new EnderecoValidator();
    }
}