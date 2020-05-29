namespace Collectio.Domain.Base.ValueObjects
{
    public class AgenciaConta
    {
        public string Conta { get; private set; }
        public string Agencia { get; private set; }

        private AgenciaConta() {}

        public AgenciaConta(string agencia, string conta)
        {
            Agencia = agencia;
            Conta = conta;
        }
    }
}