using Collectio.Domain.Base;
using Collectio.Domain.ClienteAggregate.Events;
using Collectio.Domain.ClienteAggregate.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.Base.Exceptions;

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

    public class ClienteCreatedEventHandler : IDomainEventHandler<ClienteCreatedEvent>
    {
        public async Task Handle(ClienteCreatedEvent notification, CancellationToken cancellationToken)
        {
            var errors = new List<string>()
            {
                "Isso ta errado..."
            };
            var dictio = new Dictionary<string, List<string>>()
            {
                { "Nome", errors }
            };

            //throw new UnprocessableEntityException(new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(dictio.ToDictionary(e => e.Key, e => new ReadOnlyCollection<string>(e.Value))));
        }
    }
}
