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

        private readonly Guid _id;
        private readonly DateTime _dataCriacao;
        private DateTime _dataAtualizacao;

        public Guid Id => _id;

        public DateTime DataCriacao => _dataCriacao;

        public DateTime DataAtualizacao => _dataAtualizacao;
    }

    public abstract class BaseTenantEntity : BaseEntity
    {
        public BaseTenantEntity(Guid id) : base(id)
        {}

        public BaseTenantEntity() : base(){}

        private TenantId _tenantId;
        public TenantId TenantId => _tenantId;
    }

    public class TenantId
    {
        private Guid _value;

        public TenantId(Guid value) 
            => _value = value;

        public static bool operator ==(TenantId tenantObj, Guid id) 
            => tenantObj.Value == id;

        public static bool operator !=(TenantId tenantObj, Guid id) 
            => tenantObj.Value != id;

        public Guid Value => _value;
    }
}
