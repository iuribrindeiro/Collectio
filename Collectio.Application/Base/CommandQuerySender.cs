using System.Linq;
using System.Threading.Tasks;
using Collectio.Application.Base.Commands;
using Collectio.Application.Base.Queries;
using MediatR;

namespace Collectio.Application.Base
{
    public class CommandQuerySender : ICommandQuerySender
    {
        private readonly IMediator _mediator;

        public CommandQuerySender(IMediator mediator) 
            => _mediator = mediator;

        public Task<R> Send<R>(Command<R> command) where R : CommandResponse 
            => _mediator.Send(command);

        public Task<QueryResult<R>> Send<Q, R>(Q query) 
            where Q : Query<R>
            where R : class 
            => _mediator.Send(query);
    }
}