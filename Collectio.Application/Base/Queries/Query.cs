using System.Linq;
using MediatR;

namespace Collectio.Application.Base.Queries
{
    public abstract class Query<R> : IRequest<QueryResult<R>> where R : class
    {}
}