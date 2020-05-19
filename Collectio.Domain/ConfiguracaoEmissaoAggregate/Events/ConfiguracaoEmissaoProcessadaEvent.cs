using Collectio.Domain.Base;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Events
{
    public class ConfiguracaoEmissaoProcessadaEvent : IDomainEvent
    {
        public string ConfiguracaoEmissaoId { get; private set; }

        public ConfiguracaoEmissaoProcessadaEvent(string configuracaoEmissaoId)
        {
            ConfiguracaoEmissaoId = configuracaoEmissaoId;
        }
    }
}