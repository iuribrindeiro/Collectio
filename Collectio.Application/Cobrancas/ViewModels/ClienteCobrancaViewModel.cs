namespace Collectio.Application.Cobrancas.ViewModels
{
    public class ClienteCobrancaViewModel
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string TenantId { get; set; }
        public string Email { get; set; }
        public TelefoneViewModel Telefone { get; set; }
        public EnderecoViewModel Endereco { get; set; }
    }
}