using System.Linq;
using MediatR;

namespace Collectio.Application.Base.Queries
{
    public interface IQuery { }

    public interface IQuery<R> : IRequest<R>, IQuery
    {}
}