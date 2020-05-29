using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Base.CommandValidators;
using Collectio.Application.Profiles;
using Collectio.Domain.CartaoCreditoAggregate;
using Collectio.Domain.ClienteAggregate;
using FluentValidation;
using System;

namespace Collectio.Application.CartoesCredito.Commands
{
    public class CreateCartaoCreditoCommand : ICommand<string>, IMapping
    {
        public string NomeProprietario { get; set; }
        public string CpfCnpjProprietario { get; set; }
        public string Numero { get; set; }
        public string CodigoSeguranca { get; set; }
        public DateTime Vencimento { get; set; }
        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCartaoCreditoCommand, CartaoCredito>()
                .ConstructUsing(vm => new CartaoCredito(vm.CpfCnpjProprietario, new DadosCartaoValueObject(vm.Numero, vm.CodigoSeguranca, vm.NomeProprietario, vm.Vencimento)));
    }

    public class CreateCartaoCreditoCommandValidator : AbstractValidator<CreateCartaoCreditoCommand>
    {
        public CreateCartaoCreditoCommandValidator(IClientesRepository clientesRepository)
        {
            RuleFor(c => c.CpfCnpjProprietario).NotEmpty().WithName("CPF/CNPJ").IsValidCpfOrCnpj();
            RuleFor(c => c.Vencimento).NotEmpty().GreaterThanOrEqualTo(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
            RuleFor(c => c.Numero).NotEmpty().WithName("Número").CreditCard();
            RuleFor(c => c.NomeProprietario).NotEmpty().WithName("Nome do proprietário");
            RuleFor(c => c.CodigoSeguranca).NotEmpty().MinimumLength(3).MaximumLength(5);
        }
    }
}
