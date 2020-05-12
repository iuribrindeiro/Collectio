using System;

namespace Collectio.Application.Base.Commands.Exceptions
{
    public class BusinessRuleCommandException : Exception
    {
        private BusinessRuleCommandException() { }

        public BusinessRuleCommandException(string message) : base(message)
        {}
    }
}
