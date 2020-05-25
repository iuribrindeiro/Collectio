namespace Collectio.Domain.Base.ValueObjects
{
    public class AgenciaConta
    {
        private string _agencia;
        private string _conta;

        public string Conta => _conta;
        public string Agencia => _agencia;

        public AgenciaConta(string agencia, string conta)
        {
            _agencia = agencia;
            _conta = conta;
        }
    }
}