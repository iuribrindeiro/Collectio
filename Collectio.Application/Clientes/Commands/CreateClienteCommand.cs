using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Profiles;
using Collectio.Application.ViewModels;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ClienteAggregate;

namespace Collectio.Application.Clientes.Commands
{
    public class CreateClienteCommand : ICommand<ClienteViewModel>, IMapTo<Cliente>
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }

        public void Mapping(Profile profile)
            => profile.CreateMap<CreateClienteCommand, Cliente>()
                .ConstructUsing(c => new Cliente(c.Nome, new CpfCnpjValueObject(c.CpfCnpj)));
    }
}