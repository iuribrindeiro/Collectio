using System;
using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Cobrancas.ViewModels;
using Collectio.Application.Profiles;
using Collectio.Domain.CobrancaAggregate;

namespace Collectio.Application.Cobrancas.Commands
{
    public class CreateCobrancaCartaoCommand : ICommand<CobrancaViewModel>, IMapTo<Cobranca>
    {
        public string ConfiguracaoEmissaoId { get; set; }

        public string CartaoCreditoId { get; set; }

        public string ClienteId { get; set; }

        public DateTime Vencimento { get; set; }

        public decimal Valor { get; set; }

        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCobrancaCartaoCommand, Cobranca>()
                .ConstructUsing(c => Cobranca.Cartao(c.Valor, c.Vencimento, c.ClienteId, c.ConfiguracaoEmissaoId, c.CartaoCreditoId));
    }
}