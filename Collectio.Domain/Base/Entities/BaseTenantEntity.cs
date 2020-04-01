using System;

namespace Collectio.Domain.Base
{
    public abstract class BaseTenantEntity : BaseEntity
    {
        protected BaseTenantEntity(Guid id) : base(id)
        {}

        protected BaseTenantEntity(): base(){}

        protected Guid _tenantId;
        public Guid TenantId => _tenantId;
    }
}