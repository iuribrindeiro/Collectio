using System.Collections.Generic;
using System.Linq;
using Collectio.Domain.Base;
using MediatR;

namespace Collectio.Application.Base.Commands
{
    public interface ICommand { }

    public interface ICommandResult<R>
    {
        bool IsValid { get; }
        R Result { get; }
        ErrorReason ErrorReason { get; }
        IReadOnlyCollection<PropertyCommandError> Errors { get; }
    }

    public class PropertyCommandError
    {
        private string _name;
        private List<PropertyCommandError> _propertyErrors;
        private List<string> _errors;

        public PropertyCommandError(string name, List<PropertyCommandError> propertyErrors)
        {
            _name = name;
            _propertyErrors = propertyErrors;
        }

        public PropertyCommandError(string name, List<string> errors)
        {
            _name = name;
            _propertyErrors = new List<PropertyCommandError>();
            _errors = errors;
        }

        public string Name => _name;
        public List<string> Error => _errors;
        public IReadOnlyCollection<PropertyCommandError> PropertyErrors => _propertyErrors;
    }

    public class CommandResult<R> : ICommandResult<R>
    {
        private R _result;
        private IReadOnlyCollection<PropertyCommandError> _errors;
        private ErrorReason _errorReason;

        public static CommandResult<R> Success(R result) 
            => new CommandResult<R>(result);

        public static CommandResult<R> BusinessRuleError(IReadOnlyCollection<PropertyCommandError> errors)
            => new CommandResult<R>(errors) { _errorReason = ErrorReason.BusinessRule };

        public static CommandResult<R> UnprocessableEntity(IReadOnlyCollection<PropertyCommandError> errors)
            => new CommandResult<R>(errors) { _errorReason = ErrorReason.UnprocessabelEntity };

        protected CommandResult() {}

        protected CommandResult(R result)
        {
            _result = result;
            _errors = new List<PropertyCommandError>();
        }

        protected CommandResult(IReadOnlyCollection<PropertyCommandError> errors) 
            => _errors = errors;

        public bool IsValid => !Errors.Any();
        public R Result => _result;
        public IReadOnlyCollection<PropertyCommandError> Errors => _errors;
        public ErrorReason ErrorReason => _errorReason;
    }

    public enum ErrorReason
    {
        UnprocessabelEntity,
        BusinessRule
    }

    public interface ICommand<T> : IRequest<ICommandResult<T>>, ICommand
    {
    }
}