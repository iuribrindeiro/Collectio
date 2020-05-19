using Collectio.Domain.Base;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Events
{
    public class ConfiguracaoEmissaoAlteradaEvent : IDomainEvent
    {
        public string ConfiguracaoEmissaoId { get; private set; }
        public string NomeEmpresaAnterior { get; private set; }
        public string AgenciaAnterior { get; private set; }
        public string ContaAnterior { get; private set; }
        public string EmailAnterior { get; private set; }
        public string TelefoneAnterior { get; private set; }
        public string DddAnterior { get; private set; }
        public string CpfCnpjAnterior { get; private set; }

        public ConfiguracaoEmissaoAlteradaEvent(
            string configuracaoEmissaoId, string nomeEmpresaAnterior, string agenciaAnterior, string cpfCnpjAnterior,
            string contaAnterior, string emailAnterior, string telefoneAnterior, string dddAnterior)
        {
            ConfiguracaoEmissaoId = configuracaoEmissaoId;
            NomeEmpresaAnterior = nomeEmpresaAnterior;
            AgenciaAnterior = agenciaAnterior;
            CpfCnpjAnterior = cpfCnpjAnterior;
            ContaAnterior = contaAnterior;
            EmailAnterior = emailAnterior;
            TelefoneAnterior = telefoneAnterior;
            DddAnterior = dddAnterior;
        }
    }
}