namespace Collectio.Domain.Base.ValueObjects
{
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