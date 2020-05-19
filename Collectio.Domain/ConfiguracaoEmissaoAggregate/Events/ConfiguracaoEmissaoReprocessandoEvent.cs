using Collectio.Domain.Base;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Events
{
    public class ConfiguracaoEmissaoReprocessandoEvent : IDomainEvent
    {
        public string ConfiguracaoEmissaoId { get; private set; }

        public ConfiguracaoEmissaoReprocessandoEvent(string configuracaoEmissaoId) 
            => ConfiguracaoEmissaoId = configuracaoEmissaoId;
    }
}