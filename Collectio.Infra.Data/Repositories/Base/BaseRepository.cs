using System;
using System.Linq;
using System.Threading.Tasks;
using Collectio.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Collectio.Infra.Data.Repositories.Base
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationContext _applicationContext;

        public BaseRepository(ApplicationContext applicationContext) 
            => _applicationContext = applicationContext;

        public async Task SaveAsync(T entity) 
            => await _applicationContext.Set<T>().AddAsync(entity);

        public async Task UpdateAsync(T entity)
        {
            if (_applicationContext.Entry(entity) != null && _applicationContext.Entry(entity).State != EntityState.Added)
                _applicationContext.Set<T>().Update(entity);
        }

        public Task<T> FindAsync(Guid id) 
            => _applicationContext.Set<T>().Where(e => e.Id == id).FirstOrDefaultAsync();

        public async Task<IQueryable<T>> ListAsync() 
            => _applicationContext.Set<T>().AsQueryable();

        public async Task DeleteAsync(Guid id) 
            => _applicationContext.Set<T>().Remove(await FindAsync(id));

        public async Task DeleteAsync(T entity) 
            => _applicationContext.Set<T>().Remove(entity);
    }
}
