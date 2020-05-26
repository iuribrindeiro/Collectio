using Collectio.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Collectio.Infra.Data.Repositories.Base
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly ApplicationContext _applicationContext;
        protected DbSet<T> _itens { get; private set; }

        public BaseRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _itens = _applicationContext.Set<T>();
        }

        public async Task SaveAsync(T entity) 
            => await _itens.AddAsync(entity);

        public async Task UpdateAsync(T entity)
        {
            if (_applicationContext.Entry(entity).IsKeySet)
            {
                var existingEntity = await FindAsync(entity.Id);
                _applicationContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                _itens.Update(entity);
            }
        }

        public Task<T> FindAsync(Guid id) 
            => _applicationContext.FindAsync<T>(id).AsTask();
        
        public bool Exists(Guid id, out T entity)
        {
            entity = _applicationContext.Find<T>(id);
            return entity;
        }

        public async Task<bool> ExistsAsync(Guid id) 
            => await _applicationContext.FindAsync<T>(id).AsTask();

        public IQueryable<T> ListAsync() 
            => _itens.AsQueryable();

        public async Task DeleteAsync(Guid id) 
            => _itens.Remove(await FindAsync(id));

        public async Task DeleteAsync(T entity) 
            => _itens.Remove(entity);
    }
}
