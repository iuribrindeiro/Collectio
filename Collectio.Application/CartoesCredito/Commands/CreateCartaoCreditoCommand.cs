using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Profiles;
using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Domain.ClienteAggregate;
using FluentValidation;
using System;

namespace Collectio.Application.CartoesCredito.Commands
{
    public class CreateCartaoCreditoCommand : ICommand<string>, IMapping
    {
        public string ClienteId { get; set; }
        public string Numero { get; set; }
        public string CodigoSeguranca { get; set; }
        public DateTime Vencimento { get; set; }
        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCartaoCreditoCommand, CartaoCredito>();
    }

    public class CreateCartaoCreditoCommandValidator : AbstractValidator<CreateCartaoCreditoCommand>
    {
        public CreateCartaoCreditoCommandValidator(IClientesRepository clientesRepository)
        {
            RuleFor(c => c.ClienteId).NotEmpty();
            RuleFor(c => c.Vencimento).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(c => c.Numero).CreditCard();
            RuleFor(c => c.CodigoSeguranca).MinimumLength(3).MaximumLength(5);
        }
    }
}
