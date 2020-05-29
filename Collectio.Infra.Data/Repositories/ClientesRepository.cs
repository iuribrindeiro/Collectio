using System.Linq;
using System.Threading.Tasks;
using Collectio.Domain.ClienteAggregate;
using Collectio.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Cliente = Collectio.Domain.ClienteAggregate.Cliente;

namespace Collectio.Infra.Data.Repositories
{
    public class ClientesRepository : BaseRepository<Cliente>, IClientesRepository
    {
        public ClientesRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }

        public Task<Cliente> FindByCpfCnpjAsync(string cpfCnpj) 
            => _itens.FirstOrDefaultAsync(c => c.CpfCnpj == cpfCnpj);
    }
}
