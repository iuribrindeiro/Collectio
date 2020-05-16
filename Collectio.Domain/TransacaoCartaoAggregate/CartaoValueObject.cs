using System;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Domain.TransacaoCartaoAggregate
{
    public class CartaoValueObject
    {
        private DateTime _vencimento;
        private string _numero;
        private string _codigoSeguranca;
        private string _nome;
        private CpfCnpjValueObject _cpfCnpjProprietario;

        public DateTime Vencimento => _vencimento;
        public string Numero => _numero;
        public string CodigoSeguranca => _codigoSeguranca;
        public string Nome => _nome;
        public CpfCnpjValueObject CpfCnpjCnpjProprietario => _cpfCnpjProprietario;

        public CartaoValueObject(DateTime vencimento, string numero, string codigoSeguranca, string nome, CpfCnpjValueObject cpfCnpjProprietario)
        {
            _vencimento = vencimento;
            _numero = numero;
            _codigoSeguranca = codigoSeguranca;
            _nome = nome;
            _cpfCnpjProprietario = cpfCnpjProprietario;
        }
    }
}