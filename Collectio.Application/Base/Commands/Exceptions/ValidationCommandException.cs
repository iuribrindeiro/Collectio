using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Collectio.Application.Base.Commands.Exceptions
{
    public class ValidationCommandException : Exception
    {
        private List<CommandPropertyError> _commandPropertyErrors;
        public IReadOnlyCollection<CommandPropertyError> CommandPropertyErrors => _commandPropertyErrors;

        private ValidationCommandException() : base("Alguns dos campos que você informou não eram válidos") 
            => _commandPropertyErrors = new List<CommandPropertyError>();

        public ValidationCommandException(IEnumerable<ValidationFailure> failures) : this()
        {
            foreach (var validationFailure in failures.GroupBy(e => e.PropertyName))
            {
                if (validationFailure.Key.Contains("."))
                {
                    var childProps = validationFailure.Key.Split('.').ToArray();
                    _commandPropertyErrors.Add(RecursiveSetPropertyError(childProps.First(), childProps, validationFailure.Select(e => e.ErrorMessage).ToList()));
                }
                else
                {
                    _commandPropertyErrors.Add(new CommandPropertyError(validationFailure.Key, validationFailure.Select(e => e.ErrorMessage).ToList()));
                }
            }
        }

        private CommandPropertyError RecursiveSetPropertyError(string currentPropName, string[] childProps, List<string> errorsMessage, int propIndexMessage = 0)
        {
            CommandPropertyError propError;
            if (propIndexMessage != childProps.Length - 1)
                return new CommandPropertyError(currentPropName, new List<CommandPropertyError>()
                {
                    RecursiveSetPropertyError(childProps[propIndexMessage + 1], childProps, errorsMessage, propIndexMessage + 1)
                });

            return new CommandPropertyError(currentPropName, errorsMessage);
        }
    }
}