using System;

namespace Collectio.Domain.Base
{
    public abstract class BaseOwnerEntity : BaseEntity
    {
        protected BaseOwnerEntity() {}

        public Guid OwnerId { get; private set; }
    }
}