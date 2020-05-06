using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentValidation;

namespace Collectio.Domain.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            _id = Guid.NewGuid();
            _dataCriacao = DateTime.Now;
            _errors = new Dictionary<string, List<string>>();
            _events = new List<IDomainEvent>();
            RunValidations();
        }

        public BaseEntity(Guid id)
        {
            _id = id;
            _errors = new Dictionary<string, List<string>>();
            _events = new List<IDomainEvent>();
            RunValidations();
        }

        protected Guid _id;
        protected DateTime _dataCriacao;
        protected DateTime _dataAtualizacao;
        private IList<IDomainEvent> _events;
        private IDictionary<string, List<string>> _errors;

        public Guid Id => _id;

        public DateTime DataCriacao => _dataCriacao;

        public DateTime DataAtualizacao => _dataAtualizacao;

        public virtual IReadOnlyCollection<IDomainEvent> Events => new ReadOnlyCollection<IDomainEvent>(_events);

        public bool IsValid => !Errors.Any();

        public virtual IReadOnlyDictionary<string, ReadOnlyCollection<string>> Errors
        {
            get
            {
                var dictionary = _errors.ToDictionary(e => e.Key, e => new ReadOnlyCollection<string>(e.Value));
                return new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(dictionary);
            }
        }
        protected void AddEvent(IDomainEvent domainEvent) 
            => _events.Add(domainEvent);

        protected virtual void RunValidations()
        {
            var result = ValidatorFactory().Validate(this);
            if (!result.IsValid)
            {
                foreach (var validationFailure in result.Errors.GroupBy(e => e.PropertyName))
                {
                    _errors.Add(validationFailure.Key, validationFailure.Select(e => e.ErrorMessage).ToList());
                }
            }
        }

        protected abstract IValidator ValidatorFactory();
    }
}
