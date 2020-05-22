using AutoMapper;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Application.Cobrancas.ViewModels
{
    public class TelefoneViewModel : IMapping
    {
        public string Ddd { get; set; }
        public string Numero { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TelefoneViewModel, Telefone>()
                .ConstructUsing(vm => new Telefone(vm.Ddd, vm.Numero));
        }
    }
}