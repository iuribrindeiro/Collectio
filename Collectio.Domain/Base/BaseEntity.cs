using System;

namespace Collectio.Domain.Base
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            _id = Guid.NewGuid();
            _dataCriacao = DateTime.Now;
        }

        public BaseEntity(Guid id) 
            => _id = id;

        protected Guid _id;
        protected DateTime _dataCriacao;
        protected DateTime _dataAtualizacao;

        public Guid Id => _id;

        public DateTime DataCriacao => _dataCriacao;

        public DateTime DataAtualizacao => _dataAtualizacao;
    }
}
