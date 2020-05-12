using System.Collections.Generic;

namespace Collectio.Application.Base.Commands.Exceptions
{
    public class CommandPropertyError
    {
        private string _name;
        private List<CommandPropertyError> _propertyErrors;
        private List<string> _errors;

        public CommandPropertyError(string name, List<CommandPropertyError> propertyErrors)
        {
            _name = name;
            _propertyErrors = propertyErrors;
        }

        public CommandPropertyError(string name, List<string> errors)
        {
            _name = name;
            _propertyErrors = new List<CommandPropertyError>();
            _errors = errors;
        }

        public string Name => _name;
        public List<string> Error => _errors;
        public IReadOnlyCollection<CommandPropertyError> PropertyErrors => _propertyErrors;
    }
}