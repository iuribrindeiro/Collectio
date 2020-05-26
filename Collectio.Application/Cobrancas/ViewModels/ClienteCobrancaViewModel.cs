using System.ComponentModel.DataAnnotations;
using Collectio.Application.Base.ViewModels;

namespace Collectio.Application.Cobrancas.ViewModels
{
    public class ClienteCobrancaViewModel
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string CpfCnpj { get; set; }
        [Required]
        public string TenantId { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public TelefoneViewModel Telefone { get; set; }
        [Required]
        public EnderecoViewModel Endereco { get; set; }
    }
}