﻿using AutoMapper;
using Collectio.Application.Profiles;
using Collectio.Domain.CobrancaAggregate;

namespace Collectio.Application.Cobrancas.ViewModels
{
    public class CartaoCreditoCobrancaViewModel : IMapping
    {
        public string Numero { get; set; }
        public string Nome { get; set; }
        public string TenantId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CartaoCreditoCobrancaViewModel, CartaoCredito>()
                .ConstructUsing(vm => new CartaoCredito(vm.Nome, vm.Numero, vm.TenantId));
        }
    }
}