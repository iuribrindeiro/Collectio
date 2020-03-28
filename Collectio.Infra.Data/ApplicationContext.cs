using Collectio.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using Collectio.Infra.CrossCutting.Services;

namespace Collectio.Infra.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly Guid _tenantId;

        public ApplicationContext(
            DbContextOptions<ApplicationContext> options, 
            ITenantIdProvider tenantIdProvider) : base(options) 
            => _tenantId = tenantIdProvider.TenantId;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<BaseTenantEntity>().HasQueryFilter(e => e.TenantId == _tenantId);
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
