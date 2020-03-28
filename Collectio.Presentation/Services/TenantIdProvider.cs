using Collectio.Infra.CrossCutting.Services;
using System;

namespace Collectio.Presentation
{
    public class TenantIdProvider : ITenantIdProvider
    {
        private readonly Guid _tenantId;

        public TenantIdProvider(Guid tenantId) 
            => _tenantId = tenantId;

        public Guid TenantId => _tenantId;
    }
}
