namespace Collectio.Domain.Base.ValueObjects
{
    public class AgenciaContaValueObject
    {
        private string _agencia;
        private string _conta;

        public string Conta => _conta;
        public string Agencia => _agencia;

        public AgenciaContaValueObject(string agencia, string conta)
        {
            _agencia = agencia;
            _conta = conta;
        }
    }
}