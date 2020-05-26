using Collectio.Application.Cobrancas.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace Collectio.Application.Cobrancas.Commands
{
    public abstract class BaseCreateCobrancaCommand
    {
        [Required]
        public string Descricao { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public DateTime Vencimento { get; set; }
        [Required]
        public string ConfiguracaoEmissorId { get; set; }
        [Required]
        public ClienteCobrancaViewModel Cliente { get; set; }
    }
}