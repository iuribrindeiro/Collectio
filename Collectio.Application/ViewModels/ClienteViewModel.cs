using Collectio.Application.Profiles;
using Collectio.Domain.ClienteAggregate;
using System;

namespace Collectio.Application.ViewModels
{
    public class ClienteViewModel : IMapFrom<Cliente>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
    }
}