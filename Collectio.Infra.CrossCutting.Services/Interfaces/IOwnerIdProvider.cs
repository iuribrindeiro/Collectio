using System;

namespace Collectio.Infra.CrossCutting.Services
{
    public interface IOwnerIdProvider
    {
        Guid OwnerId { get; }
    }
}
