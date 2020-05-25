using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Application.Base.ViewModels
{
    public class AgenciaContaViewModel : IMapping
    {
        [Required]
        public string Agencia { get; set; }

        [Required]
        public string Conta { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AgenciaContaViewModel, AgenciaConta>()
                .ConstructUsing(vm => new AgenciaConta(vm.Agencia, vm.Conta));
        }
    }
}