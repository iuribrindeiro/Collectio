using System;

namespace Collectio.Application.Base.Commands
{
    public class CommandResponseData<T> : CommandResponse
    {
        private T _data;
        public T Data => _data;

        public CommandResponseData()
        {}

        protected CommandResponseData(T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            _data = data;
        }

        protected CommandResponseData(T data, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            Message = message;
            _data = data;
        }

        public static CommandResponseData<T> Success(T data) => new CommandResponseData<T>(data);

        public static CommandResponseData<T> Success(T data, string message) => new CommandResponseData<T>(data, message);
    }
}