namespace Collectio.Domain.Base.ValueObjects
{
    public class ContaBancoValueObject
    {
        private string _agencia;
        private string _conta;
        private string _banco;

        public string Conta => _conta;
        public string Agencia => _agencia;
        public string Banco => _banco;

        public ContaBancoValueObject(string agencia, string conta, string banco)
        {
            _agencia = agencia;
            _conta = conta;
            _banco = banco;
        }
    }

    public class CnpjValueObject
    {
        private string _value;
        public string Value => _value;

        public CnpjValueObject(string cnpj) 
            => _value = cnpj;

        public static bool operator ==(CnpjValueObject a, CnpjValueObject b)
            => a.Value == b.Value;

        public static bool operator !=(CnpjValueObject a, CnpjValueObject b)
            => a.Value != b.Value;
    }
}