using System;
using System.Linq;
using System.Threading.Tasks;
using Collectio.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Collectio.Infra.Data.Repositories.Base
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly ApplicationContext _applicationContext;

        public BaseRepository(ApplicationContext applicationContext) 
            => _applicationContext = applicationContext;

        public async Task SaveAsync(T entity) 
            => await _applicationContext.Set<T>().AddAsync(entity);

        private async Task LoadEntity(T entity) 
            => await FindAsync(entity.Id);

        public async Task UpdateAsync(T entity)
        {
            if (_applicationContext.Entry(entity).IsKeySet)
            {
                var existingEntity = await FindAsync(entity.Id);
                _applicationContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                _applicationContext.Set<T>().Update(entity);
            }
        }

        public Task<T> FindAsync(Guid id) 
            => _applicationContext.Set<T>().Where(e => e.Id == id).FirstOrDefaultAsync();

        public Task<bool> Exists(Guid id) 
            => _applicationContext.Set<T>().AnyAsync(e => e.Id == id);

        public async Task<IQueryable<T>> ListAsync() 
            => _applicationContext.Set<T>().AsQueryable();

        public async Task DeleteAsync(Guid id) 
            => _applicationContext.Set<T>().Remove(await FindAsync(id));

        public async Task DeleteAsync(T entity) 
            => _applicationContext.Set<T>().Remove(entity);
    }
}
