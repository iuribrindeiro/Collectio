using System;

namespace Collectio.Domain.Base.Entities
{
    public class Endereco : BaseTenantEntity
    {
        private string _rua;
        private string _numero;
        private string _bairro;
        private string _cep;
        private Guid _cidadeId;
        private Cidade _cidade;

        public string Rua => _rua;
        public string Numero => _numero;
        public string Bairro => _bairro;
        public string Cep => _cep;
        public Cidade Cidade => _cidade;
        public Guid CidadeId => _cidadeId;

        public Endereco(string rua, string numero, string bairro, string cep, Guid cidadeId)
        {
            _rua = rua;
            _numero = numero;
            _bairro = bairro;
            _cep = cep;
            _cidadeId = cidadeId;
        }
    }
}