using Collectio.Application.Base.Commands.Exceptions;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Collectio.Application.Base.Commands
{
    public class CommandValidator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public CommandValidator(IEnumerable<IValidator<TRequest>> validators)
            => _validators = validators;

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext(request);

                var failures = _validators
                    .Select(v => v.Validate(context))
                    .Where(v => !v.IsValid)
                    .SelectMany(v => v.Errors).ToList();

                if (failures.Any())
                    throw new ValidationCommandException(failures);
            }

            return next();
        }
    }
}
