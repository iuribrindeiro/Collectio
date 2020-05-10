namespace Collectio.Domain.Base.ValueObjects
{
    public class CpfValueObject
    {
        private string _value;
        public string Value => _value;

        public CpfValueObject(string cnpj)
            => _value = cnpj;

        public static bool operator ==(CpfValueObject a, CpfValueObject b)
            => a.Value == b.Value;

        public static bool operator !=(CpfValueObject a, CpfValueObject b)
            => a.Value != b.Value;
    }
}