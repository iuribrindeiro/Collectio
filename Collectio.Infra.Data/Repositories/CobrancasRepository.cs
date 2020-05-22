using Collectio.Domain.CobrancaAggregate;
using Collectio.Infra.Data.Repositories.Base;

namespace Collectio.Infra.Data.Repositories
{
    public class CobrancasRepository : BaseRepository<Cobranca>, ICobrancasRepository
    {
        public CobrancasRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
    }
}