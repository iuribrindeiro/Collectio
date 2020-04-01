using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Collectio.Application.Base.Commands
{
    public class CommandResponse
    {
        protected CommandResponse() 
            => Errors = new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(new Dictionary<string, ReadOnlyCollection<string>>());

        public static R Success<R>() where R : CommandResponse => new CommandResponse() as R;
        public static R Success<R>(string message) where R : CommandResponse => new CommandResponse() { Message = message } as R;

        public static R UnprocessableEntity<R>(string message, IReadOnlyDictionary<string, ReadOnlyCollection<string>> errors) where R : CommandResponse
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            return new CommandResponse()
            {
                ErrorReason = Commands.ErrorReason.UnprocessableEntity,
                Message = message,
                Errors = errors
            } as R;
        }

        public static R BusinessRulesFailure<R>(string message, IReadOnlyDictionary<string, ReadOnlyCollection<string>> errors) where R : CommandResponse
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            return new CommandResponse()
            {
                ErrorReason = Commands.ErrorReason.BusinessRulesFailure,
                Message = message,
                Errors = errors
            } as R;
        }

        public static R UnexpectedError<R>(string message) where R : CommandResponse
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            return new CommandResponse()
            {
                ErrorReason = Commands.ErrorReason.UnexpectedError,
                Message = message,
                Errors = new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(new Dictionary<string, ReadOnlyCollection<string>>())
            } as R;
        }

        public string Message { get; protected set; }

        public IReadOnlyDictionary<string, ReadOnlyCollection<string>> Errors { get; protected set; }

        public ErrorReason? ErrorReason { get; protected set; }

        public bool IsSuccess => ErrorReason == null;
    }

    public enum ErrorReason
    {
        UnprocessableEntity,
        BusinessRulesFailure,
        UnexpectedError
    }
}