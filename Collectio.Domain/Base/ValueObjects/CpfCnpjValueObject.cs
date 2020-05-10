namespace Collectio.Domain.Base.ValueObjects
{
    public class CpfCnpjValueObject
    {
        private string _value;
        public string Value => _value;

        public CpfCnpjValueObject(string cpfCnpj)
            => _value = cpfCnpj;

        public static bool operator ==(CpfCnpjValueObject a, CpfCnpjValueObject b)
            => a.Value == b.Value;

        public static bool operator !=(CpfCnpjValueObject a, CpfCnpjValueObject b)
            => a.Value != b.Value;
    }
}