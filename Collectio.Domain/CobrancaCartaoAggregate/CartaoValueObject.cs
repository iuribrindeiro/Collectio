using System;
using Collectio.Domain.Base.Entities;

namespace Collectio.Domain.CobrancaCartaoAggregate
{
    public class CartaoValueObject : IUnpersistentProperty
    {
        private DateTime _vencimento;
        private string _numero;
        private string _codigoSeguranca;
        private string _nome;
        private string _cpfProprietario;

        public DateTime Vencimento => _vencimento;
        public string Numero => _numero;
        public string CodigoSeguranca => _codigoSeguranca;
        public string Nome => _nome;
        public string CpfProprietario => _cpfProprietario;

        public CartaoValueObject(DateTime vencimento, string numero, string codigoSeguranca, string nome)
        {
            _vencimento = vencimento;
            _numero = numero;
            _codigoSeguranca = codigoSeguranca;
            _nome = nome;
        }
    }
}