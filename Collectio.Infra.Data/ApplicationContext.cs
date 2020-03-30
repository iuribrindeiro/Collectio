using Collectio.Domain.Base;
using Collectio.Infra.CrossCutting.Services;
using Collectio.Infra.CrossCutting.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Collectio.Infra.Data.EntitiyTypes.Base;

namespace Collectio.Infra.Data
{
    public class ApplicationContext : DbContext, IUnitOfWork
    {
        private readonly Guid _tenantId;

        public ApplicationContext(
            DbContextOptions<ApplicationContext> options, 
            ITenantIdProvider tenantIdProvider) : base(options) 
            => _tenantId = tenantIdProvider.TenantId;

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

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && e.State == EntityState.Modified && e.State == EntityState.Added);

            foreach (var entity in entities)
            {
                entity.CurrentValues["_dataAtualizacao"] = DateTime.Now;
                entity.CurrentValues["_dataCriacao"] = entity.OriginalValues["_dataCriacao"] ?? (entity.Entity as BaseEntity).DataCriacao;
                if (entity.Entity is BaseTenantEntity)
                    entity.CurrentValues["_tenantId"] = _tenantId;
            }
                

            return base.SaveChanges();
        }
    }
}
