using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Application.Base.ViewModels
{
    public class TelefoneViewModel : IMapping
    {
        [Required]
        public string Ddd { get; set; }
        [Required]
        public string Numero { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TelefoneViewModel, Telefone>()
                .ConstructUsing(vm => new Telefone(vm.Ddd, vm.Numero));
        }
    }
}