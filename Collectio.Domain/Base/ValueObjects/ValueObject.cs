namespace Collectio.Domain.Base.ValueObjects
{
    public abstract class ValueObject
    {
        public static implicit operator bool(ValueObject a) 
            => a != null;
    }
}