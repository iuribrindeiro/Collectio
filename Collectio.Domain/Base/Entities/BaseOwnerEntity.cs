using System;

namespace Collectio.Domain.Base
{
    public abstract class BaseOwnerEntity : BaseEntity
    {
        protected BaseOwnerEntity(): base(){}

        private Guid _ownerId;
        public Guid OwnerId => _ownerId;
    }
}