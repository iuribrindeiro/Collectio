using System.Threading.Tasks;
using Collectio.Domain.Base;

namespace Collectio.Domain.ClienteAggregate
{
    public interface IClientesRepository : IRepository<Cliente>
    {
        Task<Cliente> FindByCpfCnpjAsync(string cpfCnpj);
    }
}
