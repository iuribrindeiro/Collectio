using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Application.Cobrancas.ViewModels
{
    public class EnderecoViewModel : ValueObject
    {
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
    }
}