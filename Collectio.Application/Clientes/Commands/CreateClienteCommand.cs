using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Base.CommandValidators;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ClienteAggregate;
using FluentValidation;

namespace Collectio.Application.Clientes.Commands
{
    public class CreateClienteCommand : ICommand<string>, IMapping
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }

        public void Mapping(Profile profile)
            => profile.CreateMap<CreateClienteCommand, Cliente>()
                .ConstructUsing(c => new Cliente(c.Nome, c.CpfCnpj));
    }

    public class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
    {
        public CreateClienteCommandValidator()
        {
            RuleFor(c => c.CpfCnpj).NotEmpty().IsValidCpfOrCnpj();
            RuleFor(c => c.Nome).NotEmpty();
        }
    }
}