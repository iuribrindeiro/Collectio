using Collectio.Application.Cobrancas.ViewModels;
using System;

namespace Collectio.Application.Cobrancas.Commands
{
    public abstract class BaseCreateCobrancaCommand
    {
        public string TenantIdCliente { get; set; }
        public string EmailCliente { get; set; }
        public TelefoneViewModel TelefoneCliente { get; set; }
        public EnderecoViewModel EnderecoCliente { get; set; }
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCnpjCliente { get; set; }
        public string ConfiguracaoEmissorId { get; set; }
    }
}