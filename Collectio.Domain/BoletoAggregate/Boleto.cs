using System;
using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Domain.BoletoAggregate
{
    public class Boleto : BaseTenantEntity, IAggregateRoot
    {
        private DateTime _vencimento;
        private decimal _valor;

        public DateTime Vencimento => _vencimento;
        public decimal Valor => _valor;
        public Emissor Emissor { get; set; }
    }

    public class Emissor : BaseTenantEntity
    {
        private string _nome;
        private CpfCnpjValueObject _cpfCnpj;
        private EmailValueObject _email;

        public string Nome => _nome;
        public CpfCnpjValueObject CpfCnpj => _cpfCnpj;
        public EmailValueObject Email => _email;
    }


    public class Endereco
    {
        private string _rua;
        private string _numero;
        private string _bairro;
        private string _cep;

        public string Rua => _rua;
        public string Numero => _numero;
        public string Bairro => _bairro;
        public string Cep => _cep;
    }

    public class EmailValueObject
    {
        private string _value;
        public string Value => _value;

        public EmailValueObject(string email) 
            => _value = email;

        public static bool operator ==(EmailValueObject a,  EmailValueObject b) 
            => a?.Value == b?.Value;

        public static bool operator !=(EmailValueObject a, EmailValueObject b)
            => a?.Value != b?.Value;
    }
}
