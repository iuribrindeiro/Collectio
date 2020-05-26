using AutoMapper;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Collectio.Application.Cobrancas.ViewModels
{
    public class EnderecoViewModel : IMapping
    {
        [Required]
        public string Rua { get; set; }
        [Required]
        public string Numero { get; set; }
        [Required]
        public string Bairro { get; set; }
        [Required]
        public string Cep { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string Uf { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EnderecoViewModel, Endereco>()
                .ConstructUsing(vm => new Endereco(vm.Rua, vm.Numero, vm.Bairro, vm.Cep, vm.Uf, vm.Cidade));
        }
    }
}