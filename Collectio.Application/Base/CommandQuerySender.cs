using Collectio.Application.Base.Commands;
using Collectio.Application.Base.Queries;
using MediatR;
using System.Threading.Tasks;

namespace Collectio.Application.Base
{
    public class CommandQuerySender : ICommandQuerySender
    {
        private readonly IMediator _mediator;

        public CommandQuerySender(IMediator mediator) 
            => _mediator = mediator;

        public Task<R> Send<R>(ICommand<R> command)
            => _mediator.Send(command);

        public Task<R> Send<Q, R>(Q query) where Q : IQuery<R>
            => _mediator.Send(query);
    }
}