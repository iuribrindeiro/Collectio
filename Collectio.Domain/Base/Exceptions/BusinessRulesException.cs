using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Collectio.Domain.Base.Exceptions
{
    public class BusinessRulesException : MultipleErrorsException
    {
        public BusinessRulesException(string message, IReadOnlyDictionary<string, ReadOnlyCollection<string>> errors) : base(message, errors)
        {}

        public BusinessRulesException(string message) : base(message)
        {}
    }
}