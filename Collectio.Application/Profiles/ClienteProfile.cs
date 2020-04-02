using AutoMapper;
using Collectio.Application.Commands;
using Collectio.Application.ViewModels;
using Collectio.Domain.ClienteAggregate;

namespace Collectio.Application.Profiles
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<CreateClienteCommand, Cliente>().ConstructUsing(e => new Cliente(e.Nome));
            CreateMap<UpdateClienteCommand, Cliente>().ConstructUsing(e => new Cliente(e.Id, e.Nome));
            CreateMap<Cliente, ClienteViewModel>();
        }
    }
}
