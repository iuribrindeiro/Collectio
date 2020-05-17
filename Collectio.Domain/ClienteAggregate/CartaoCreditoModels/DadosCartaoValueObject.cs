using System;

namespace Collectio.Domain.ClienteAggregate.CartaoCreditoModels
{
    public class DadosCartaoValueObject
    {
        private string _numero;
        private string _codigoSeguranca;
        private string _nomeProprietario;
        private DateTime _vencimento;

        public string Numero => _numero;
        public string CodigoSeguranca => _codigoSeguranca;
        public string NomeProprietario => _nomeProprietario;
        public DateTime Vencimento => _vencimento;

        public DadosCartaoValueObject(string numero, string codigoSeguranca, string nomeProprietario, DateTime vencimento)
        {
            _numero = numero;
            _codigoSeguranca = codigoSeguranca;
            _nomeProprietario = nomeProprietario;
            _vencimento = vencimento;
        }
    }
}