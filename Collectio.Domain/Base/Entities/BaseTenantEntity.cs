using System;

namespace Collectio.Domain.Base
{
    public abstract class BaseTenantEntity : BaseEntity
    {
        protected BaseTenantEntity(): base(){}

        private Guid _tenantId;
        public Guid TenantId => _tenantId;
    }
}