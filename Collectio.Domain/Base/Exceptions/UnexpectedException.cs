using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Collectio.Domain.Base.Exceptions
{
    public class UnprocessableEntityException : MultipleErrorsException
    {
        public UnprocessableEntityException(IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors) : base("Alguns dos campos que você informou não eram válidos")
        {}
    }

    public abstract class MultipleErrorsException : Exception
    {
        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Errors { get; protected set; }

        public MultipleErrorsException(string message, IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors) : base(message)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            Errors = errors;
        }

        public MultipleErrorsException(string message) : base(message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            Errors = new ReadOnlyDictionary<string, IReadOnlyCollection<string>>(new Dictionary<string, IReadOnlyCollection<string>>());
        }
    }

    public class BusinessRulesException : MultipleErrorsException
    {
        public BusinessRulesException(string message, IReadOnlyDictionary<string, IReadOnlyCollection<string>> errors) : base(message, errors)
        {}

        public BusinessRulesException(string message) : base(message)
        {}
    }
}
