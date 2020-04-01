using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Collectio.Domain.Base.Exceptions
{
    public abstract class MultipleErrorsException : Exception
    {
        public IReadOnlyDictionary<string, ReadOnlyCollection<string>> Errors { get; protected set; }

        public MultipleErrorsException(string message, IReadOnlyDictionary<string, ReadOnlyCollection<string>> errors) : base(message)
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

            Errors = new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(new Dictionary<string, ReadOnlyCollection<string>>());
        }
    }
}