using MediatR;

namespace Collectio.Application.Base.Queries
{
    public interface IQueryHandler<Q, R> : IRequestHandler<Q, R>
        where Q : IQuery<R>
    {}
}