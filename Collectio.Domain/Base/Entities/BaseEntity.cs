using System;
using System.Collections.Generic;

namespace Collectio.Domain.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            _events = new List<IDomainEvent>();
        }

        private List<IDomainEvent> _events;

        public Guid Id { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public DateTime? DataAtualizacao { get; private set; }

        public virtual IReadOnlyCollection<IDomainEvent> Events => _events;

        protected void AddEvent(IDomainEvent domainEvent) 
            => _events.Add(domainEvent);

        public static implicit operator bool(BaseEntity e)
            => e != null;

        public static bool operator ==(BaseEntity a, BaseEntity b) 
            => a?.Id == b?.Id;

        public static bool operator !=(BaseEntity a, BaseEntity b)
            => a?.Id != b?.Id;
    }
}
