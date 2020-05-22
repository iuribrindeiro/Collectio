using AutoMapper;
using Collectio.Application.Profiles;
using System;
using Collectio.Domain.ClienteAggregate;

namespace Collectio.Application.ViewModels
{
    public class ClienteViewModel : IMapping
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public void Mapping(Profile profile) 
            => profile.CreateMap<ClienteViewModel, Cliente>();
    }
}