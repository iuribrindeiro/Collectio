using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Infra.Data.Repositories.Base;

namespace Collectio.Infra.Data.Repositories
{
    public class CartaoCreditoRepository : BaseRepository<CartaoCredito>, ICartaoCreditoRepository
    {
        public CartaoCreditoRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
    }
}