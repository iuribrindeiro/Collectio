using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Commands;
using Collectio.Domain.Base;
using Collectio.Domain.ClienteAggregate;
using Collectio.Infra.CrossCutting.Services;

namespace Collectio.Application.Profiles
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<CreateClienteCommand, Cliente>().ConstructUsing(e => new Cliente(e.Nome));
            CreateMap<UpdateClienteCommand, Cliente>().ConstructUsing(e => new Cliente(e.Id, e.Nome));
        }
    }
}
