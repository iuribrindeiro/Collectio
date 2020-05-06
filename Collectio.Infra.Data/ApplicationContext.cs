using Collectio.Domain.Base;
using Collectio.Infra.CrossCutting.Services;
using Collectio.Infra.CrossCutting.Services.Interfaces;
using Collectio.Infra.Data.EntitiyTypes.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Collectio.Infra.Data
{
    public class ApplicationContext : DbContext, IUnitOfWork
    {
        private readonly IDomainEventEmitter _domainEventEmitter;
        private readonly Guid _tenantId;
        private IList<IDomainEvent> _eventsSent;

        public ApplicationContext(
            DbContextOptions<ApplicationContext> options, 
            ITenantIdProvider tenantIdProvider, IDomainEventEmitter domainEventEmitter) : base(options)
        {
            _domainEventEmitter = domainEventEmitter;
            _tenantId = tenantIdProvider.TenantId;
            _eventsSent = new List<IDomainEvent>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(BaseEntityTypeConfiguration<>)));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => (bool)e.ClrType?.IsAssignableFrom(typeof(BaseTenantEntity))))
            {
                Expression<Func<BaseTenantEntity, bool>> filter = e => e.TenantId == _tenantId;
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await UpdatePrivateFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task UpdatePrivateFields()
        {
            var dataAtual = DateTime.Now;
            foreach (var entity in ModifiedAndAddedEntities())
            {
                var dataCriacaoOriginal = (entity.OriginalValues["DataCriacao"] as DateTime?);
                entity.CurrentValues["DataAtualizacao"] = dataAtual;
                entity.CurrentValues["DataCriacao"] = dataCriacaoOriginal != null && dataCriacaoOriginal != DateTime.MinValue ? dataCriacaoOriginal : dataAtual;
                if (entity.Entity is BaseTenantEntity)
                    entity.CurrentValues["TenantId"] = _tenantId;
            }

            await _domainEventEmitter.PublishAsync(PendingEvents().ToArray());
            _eventsSent = _eventsSent.Concat(PendingEvents()).ToList();

            if (ModifiedAndAddedEntities().Any() || PendingEvents().Any())
                await UpdatePrivateFields();
        }

        private IEnumerable<IDomainEvent> PendingEvents() 
            => DomainEvents().Where(es => !_eventsSent.Contains(es));

        private IEnumerable<EntityEntry> EntityEntries()
        {
            var entities = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity);
            return entities;
        }

        private IEnumerable<EntityEntry> ModifiedAndAddedEntities()
        {
            var entities = EntityEntries();
            var modifiedAndAdded = entities.Where(e => (e.State == EntityState.Modified || e.State == EntityState.Added) &&
                                                       ((e.Entity as BaseEntity).DataCriacao == DateTime.MinValue || (e.Entity as BaseEntity).DataAtualizacao == DateTime.MinValue || 
                                                        e.Entity is BaseTenantEntity && (e.Entity as BaseTenantEntity).TenantId == Guid.Empty));
            return modifiedAndAdded;
        }

        private IEnumerable<IDomainEvent> DomainEvents()
        {
            var entities = EntityEntries();
            var domainEvents = entities.SelectMany(e => (e.Entity as BaseEntity).Events);
            return domainEvents;
        }
    }
}
