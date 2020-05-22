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
        protected DbSet<T> Itens { get; private set; }

        public BaseRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            Itens = _applicationContext.Set<T>();
        }

        public async Task SaveAsync(T entity) 
            => await Itens.AddAsync(entity);

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
                Itens.Update(entity);
            }
        }

        public Task<T> FindAsync(Guid id) 
            => Itens.Where(e => e.Id == id).FirstOrDefaultAsync();

        public Task<bool> Exists(Guid id) 
            => Itens.AnyAsync(e => e.Id == id);

        public async Task<IQueryable<T>> ListAsync() 
            => Itens.AsQueryable();

        public async Task DeleteAsync(Guid id) 
            => Itens.Remove(await FindAsync(id));

        public async Task DeleteAsync(T entity) 
            => Itens.Remove(entity);
    }
}
