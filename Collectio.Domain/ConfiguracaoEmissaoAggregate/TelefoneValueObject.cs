namespace Collectio.Domain.ConfiguracaoEmissaoAggregate
{
    public class TelefoneValueObject
    {
        private string _ddd;
        private string _telefone;

        public string Ddd => _ddd;
        public string Telefone => _telefone;

        public TelefoneValueObject(string ddd, string telefone)
        {
            _ddd = ddd;
            _telefone = telefone;
        }
    }
}