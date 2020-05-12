using System;
using System.Collections.Generic;

namespace Collectio.Domain.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            _id = Guid.NewGuid();
            _dataCriacao = DateTime.Now;
            _events = new List<IDomainEvent>();
        }

        public BaseEntity(Guid id)
        {
            _id = id;
            _events = new List<IDomainEvent>();
        }

        private Guid _id;
        private DateTime _dataCriacao;
        private DateTime _dataAtualizacao;
        private List<IDomainEvent> _events;

        public Guid Id => _id;

        public DateTime DataCriacao => _dataCriacao;

        public DateTime DataAtualizacao => _dataAtualizacao;

        public virtual IReadOnlyCollection<IDomainEvent> Events => _events;

        protected void AddEvent(IDomainEvent domainEvent) 
            => _events.Add(domainEvent);

        public static implicit operator bool(BaseEntity e)
            => e != null;
    }
}
