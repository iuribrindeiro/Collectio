﻿using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Cobrancas.ViewModels;
using Collectio.Application.Profiles;
using Collectio.Domain.CobrancaAggregate;
using System;

namespace Collectio.Application.Cobrancas.Commands
{
    public class CreateCobrancaBoletoCommand : ICommand<CobrancaViewModel>, IMapTo<Cobranca>
    {
        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCobrancaBoletoCommand, Cobranca>()
                .ConstructUsing(c => Cobranca.Boleto(c.Valor, c.Vencimento, c.ClienteId, c.ContaBancariaId));

        public decimal Valor { get; set; }

        public DateTime Vencimento { get; set; }

        public string ClienteId { get; set; }

        public string EmissorId { get; set; }

        public string ContaBancariaId { get; set; }
    }
}