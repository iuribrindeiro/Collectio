using System;

namespace Collectio.Infra.CrossCutting.Services
{
    public interface ITenantIdProvider
    {
        Guid TenantId { get; }
    }
}
