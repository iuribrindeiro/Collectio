namespace Collectio.Domain.Base.ValueObjects
{
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