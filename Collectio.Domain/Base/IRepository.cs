using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectio.Domain.Base
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task SaveAsync(T entity);
        Task UpdateAsync(T entity);
        Task<T> FindAsync(Guid id);
        Task<IQueryable<T>> ListAsync();
        Task DeleteAsync(Guid id);
        Task DeleteAsync(T entity);
    }
}
