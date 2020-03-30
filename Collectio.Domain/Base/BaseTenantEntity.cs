using System;

namespace Collectio.Domain.Base
{
    public abstract class BaseTenantEntity : BaseEntity
    {
        public BaseTenantEntity(Guid id) : base(id)
        {}

        public BaseTenantEntity() : base(){}

        protected Guid _tenantId;
        public Guid TenantId => _tenantId;
    }
}