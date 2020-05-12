using System.Linq;
using Collectio.Domain.Base;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Collectio.Domain.Test
{
    [TestClass]
    public class BaseEntityUnitTest
    {
        [TestMethod]
        public void OsNiveisErroEstaoSendoPreenchidosCorretamente()
        {
            var testeBaseEntity = new TesteBaseEntity("", new SomeValueObject(""));
            testeBaseEntity.Validate();
            var errors = testeBaseEntity.Errors;
            
            
            Assert.IsTrue(errors.Any(e => e.Name == nameof(TesteBaseEntity.Somefield)), 
                $"A propriedade {nameof(TesteBaseEntity.Somefield)} deve estar no primeiro nivel de errors");

            Assert.IsFalse(errors.SelectMany(e => e.PropertyErrors).Any(e => e.Name == nameof(TesteBaseEntity.Somefield)),
                $"A propriedade {nameof(TesteBaseEntity.SomeValueObject.SomeValueObjectField)} não deve estar no primeiro nivel");

            Assert.IsTrue(errors.SelectMany(e => e.PropertyErrors).Any(e => e.Name == nameof(TesteBaseEntity.SomeValueObject.SomeValueObjectField)), 
                $"A propriedade {nameof(TesteBaseEntity.SomeValueObject.SomeValueObjectField)} deve estar no segundo nivel de errors");

            Assert.IsFalse(errors.Any(e => e.Name == nameof(TesteBaseEntity.SomeValueObject.SomeValueObjectField)),
                $"A propriedade {nameof(TesteBaseEntity.SomeValueObject.SomeValueObjectField)} não deve estar no primeiro nivel");
        }
    }

    public class SomeValueObject
    {
        private string _someValueObjectField;

        public SomeValueObject(string someValueObjectField) 
            => _someValueObjectField = someValueObjectField;

        public string SomeValueObjectField => _someValueObjectField;
    }

    public class TesteBaseEntity : BaseEntity
    {
        private string _somefield;
        private SomeValueObject _someValueObject;

        public TesteBaseEntity(string somefield, SomeValueObject someValueObject)
        {
            _somefield = somefield;
            _someValueObject = someValueObject;
        }

        public string Somefield => _somefield;
        public SomeValueObject SomeValueObject => _someValueObject;

        protected override IValidator ValidatorFactory() 
            => new TesteBaseEntityValidator();
    }

    public class SomeValueObjectValidator : AbstractValidator<SomeValueObject>
    {
        public SomeValueObjectValidator()
        {
            RuleFor(e => e.SomeValueObjectField).NotEmpty();
        }
    }

    public class TesteBaseEntityValidator : AbstractValidator<TesteBaseEntity>
    {
        public TesteBaseEntityValidator()
        {
            RuleFor(e => e.Somefield).NotEmpty();
            RuleFor(e => e.SomeValueObject).SetValidator(new SomeValueObjectValidator());
        }
    }
}
