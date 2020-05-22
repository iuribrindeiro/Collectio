using AutoMapper;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Application.Cobrancas.ViewModels
{
    public class EnderecoViewModel : IMapping
    {
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EnderecoViewModel, Endereco>()
                .ConstructUsing(vm => new Endereco(vm.Rua, vm.Numero, vm.Bairro, vm.Cep, vm.Uf, vm.Cidade));
        }
    }
}