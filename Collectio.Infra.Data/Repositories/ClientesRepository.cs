using Collectio.Domain.ClienteAggregate;
using Collectio.Infra.Data.Repositories.Base;

namespace Collectio.Infra.Data.Repositories
{
    public class ClientesRepository : BaseRepository<Cliente>, IClientesRepository
    {
        public ClientesRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
    }
}
