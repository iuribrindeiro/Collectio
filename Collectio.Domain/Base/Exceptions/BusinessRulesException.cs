using System;

namespace Collectio.Domain.Base.Exceptions
{
    public class BusinessRulesException : Exception
    {
        private BusinessRulesException() {}

        public BusinessRulesException(string message) : base(message) {}
    }
}