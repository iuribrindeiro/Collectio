using Collectio.Infra.CrossCutting.Services;
using System;

namespace Collectio.Presentation
{
    public class OwnerIdProvider : IOwnerIdProvider
    {
        private readonly Guid _ownerId;

        public OwnerIdProvider(Guid ownerId) 
            => _ownerId = ownerId;

        public Guid OwnerId => _ownerId;
    }
}
