using System.Linq;
using Collectio.Application.Base.Queries;
using Collectio.Application.ViewModels;

namespace Collectio.Application.Queries
{
    public class ClienteQueryRequest : IQuery<IQueryable<ClienteViewModel>>
    {}
}