using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Collectio.Domain.Base.Exceptions;
using FluentValidation;

namespace Collectio.Domain.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            _id = Guid.NewGuid();
            _dataCriacao = DateTime.Now;
            _errors = new List<PropertyError>();
            _events = new List<IDomainEvent>();
        }

        public BaseEntity(Guid id)
        {
            _id = id;
            _errors = new List<PropertyError>();
            _events = new List<IDomainEvent>();
        }

        private Guid _id;
        private DateTime _dataCriacao;
        private DateTime _dataAtualizacao;
        private List<IDomainEvent> _events;
        private List<PropertyError> _errors;

        public Guid Id => _id;

        public DateTime DataCriacao => _dataCriacao;

        public DateTime DataAtualizacao => _dataAtualizacao;

        public virtual IReadOnlyCollection<IDomainEvent> Events => _events;

        public bool IsValid => !Errors.Any();
        public virtual IReadOnlyCollection<PropertyError> Errors => _errors;
        protected void AddEvent(IDomainEvent domainEvent) 
            => _events.Add(domainEvent);

        private PropertyError RecursiveSetPropertyError(string currentPropName, string[] childProps, List<string> errorsMessage, int propIndexMessage = 0)
        {
            PropertyError propError;
            if (propIndexMessage != childProps.Length - 1)
                return new PropertyError(currentPropName, new List<PropertyError>()
                {
                    RecursiveSetPropertyError(childProps[propIndexMessage + 1], childProps, errorsMessage, propIndexMessage + 1)
                });

            return new PropertyError(currentPropName, errorsMessage);
        }

        public void Validate()
        {
            var result = ValidatorFactory().Validate(this);
            if (!result.IsValid)
            {
                foreach (var validationFailure in result.Errors.GroupBy(e => e.PropertyName))
                {
                    if (validationFailure.Key.Contains("."))
                    {
                        var childProps = validationFailure.Key.Split('.').ToArray();
                        _errors.Add(RecursiveSetPropertyError(childProps.First(), childProps, validationFailure.Select(e => e.ErrorMessage).ToList()));
                    }
                    else
                    {
                        _errors.Add(new PropertyError(validationFailure.Key, validationFailure.Select(e => e.ErrorMessage).ToList()));
                    }
                }
            }
        }

        protected abstract IValidator ValidatorFactory();

        public static implicit operator bool(BaseEntity e)
            => e != null;
    }

    public class PropertyError
    {
        private string _name;
        private List<PropertyError> _propertyErrors;
        private List<string> _errors;

        public PropertyError(string name, List<PropertyError> propertyErrors)
        {
            _name = name;
            _propertyErrors = propertyErrors;
        }

        public PropertyError(string name, List<string> errors)
        {
            _name = name;
            _propertyErrors = new List<PropertyError>();
            _errors = errors;
        }

        public string Name => _name;
        public List<string> Error => _errors;
        public IReadOnlyCollection<PropertyError> PropertyErrors => _propertyErrors;
    }
}
