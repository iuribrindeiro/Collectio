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

                    if (_commandPropertyErrors.Any(c => c.Name == childProps.First()))
                    {
                        AddPropertyErrorRecursive(childProps[1], _commandPropertyErrors.FirstOrDefault(c => c.Name == childProps.First()), childProps, validationFailure.Select(e => e.ErrorMessage).ToList());    
                    }
                    else
                    {
                        _commandPropertyErrors.Add(RecursiveSetPropertyError(childProps.First(), childProps, validationFailure.Select(e => e.ErrorMessage).ToList()));
                    }   
                }
                else
                {
                    _commandPropertyErrors.Add(new CommandPropertyError(validationFailure.Key, validationFailure.Select(e => e.ErrorMessage).ToList()));
                }
            }
        }

        private void AddPropertyErrorRecursive(string currentPropName, CommandPropertyError commandPropertyError, string[] childProps, List<string> errorsMessage, int propIndexMessage = 1)
        {
            if (commandPropertyError.PropertyErrors.Any(c => c.Name == currentPropName))
            {
                AddPropertyErrorRecursive(childProps[propIndexMessage + 1], commandPropertyError.PropertyErrors.FirstOrDefault(c => c.Name == currentPropName), childProps, errorsMessage, propIndexMessage++);
                return;
            }

            propIndexMessage = childProps.LastOrDefault() == currentPropName ? propIndexMessage + 1 : propIndexMessage;

            commandPropertyError.AddCommandPropertyError(RecursiveSetPropertyError(currentPropName, childProps, errorsMessage, propIndexMessage));
        }

        private CommandPropertyError RecursiveSetPropertyError(string currentPropName, string[] childProps, List<string> errorsMessage, int propIndexMessage = 0)
        {
            if (propIndexMessage != childProps.Length - 1)
            {
                var propertyErrors = new List<CommandPropertyError>()
                {
                    RecursiveSetPropertyError(
                        childProps[propIndexMessage + 1], 
                        childProps, 
                        errorsMessage,
                        propIndexMessage + 1)
                };
                return new CommandPropertyError(currentPropName, propertyErrors);
            }
                

            return new CommandPropertyError(currentPropName, errorsMessage);
        }
    }
}