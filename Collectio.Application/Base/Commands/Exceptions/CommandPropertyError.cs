using System.Collections.Generic;

namespace Collectio.Application.Base.Commands.Exceptions
{
    public class CommandPropertyError
    {
        private List<CommandPropertyError> _propertyErrors;

        public CommandPropertyError(string name, List<CommandPropertyError> propertyErrors)
        {
            Name = name;
            _propertyErrors = propertyErrors;
        }

        public CommandPropertyError(string name, List<string> errors)
        {
            Name = name;
            _propertyErrors = new List<CommandPropertyError>();
            Error = errors;
        }

        public void AddCommandPropertyError(CommandPropertyError propertyError)
        {
            _propertyErrors.Add(propertyError);
        }

        public string Name { get; private set; }

        public List<string> Error { get; private set; }

        public IReadOnlyCollection<CommandPropertyError> PropertyErrors => _propertyErrors;
    }
}