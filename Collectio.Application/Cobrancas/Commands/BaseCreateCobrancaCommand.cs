using Collectio.Application.Cobrancas.ViewModels;
using System;

namespace Collectio.Application.Cobrancas.Commands
{
    public abstract class BaseCreateCobrancaCommand
    {
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; }
        public string ConfiguracaoEmissorId { get; set; }
        public ClienteCobrancaViewModel Cliente { get; set; }
    }
}