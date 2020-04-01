using Collectio.Domain.Base;
using FluentValidation;
using System;

namespace Collectio.Domain.ClienteAggregate
{
    public class Cliente : BaseTenantEntity
    {
        //TO CREATE
        public Cliente(string nome) : base()
        {
            Nome = nome;
            AddEvent(new ClienteCreatedEvent(this));
        }

        //TO UPDATE
        public Cliente(Guid id, string nome) : base(id)
        {
            Nome = nome;
            AddEvent(new ClienteUpdatedEvent(this));
        }

        public string Nome { get; set; }

        protected override IValidator ValidatorFactory() 
            => new ClienteValidator();
    }

    public class ClienteUpdatedEvent : IDomainEvent
    {
        private Cliente _cliente;
        public Cliente cliente => _cliente;

        public ClienteUpdatedEvent(Cliente cliente) 
            => _cliente = cliente;
    }

    public class ClienteCreatedEvent : IDomainEvent
    {
        private readonly Cliente _cliente;
        public Cliente Cliente => _cliente;

        public ClienteCreatedEvent(Cliente cliente) 
            => _cliente = cliente;
    }

    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleFor(e => e.Nome).NotEmpty().NotNull();
        }
    }
}
