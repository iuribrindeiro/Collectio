namespace Collectio.Domain.Base.ValueObjects
{
    public class Endereco : ValueObject
    {
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Cep { get; private set; }
        public string Cidade { get; private set; }
        public string Uf { get; private set; }

        public Endereco(string rua, string numero, string bairro, string cep, string uf, string cidade)
        {
            Rua = rua;
            Numero = numero;
            Bairro = bairro;
            Cep = cep;
            Cidade = cidade;
            Uf = uf;
        }
    }
}