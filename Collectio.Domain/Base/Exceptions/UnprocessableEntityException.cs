using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Collectio.Domain.Base.Exceptions
{
    public class UnprocessableEntityException : MultipleErrorsException
    {
        public UnprocessableEntityException(IReadOnlyDictionary<string, ReadOnlyCollection<string>> errors) : base("Alguns dos campos que você informou não eram válidos", errors)
        {}
    }
}