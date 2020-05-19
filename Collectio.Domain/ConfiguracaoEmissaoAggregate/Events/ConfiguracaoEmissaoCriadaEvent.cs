using Collectio.Domain.Base;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Events
{
    public class ConfiguracaoEmissaoCriadaEvent : IDomainEvent
    {
        public string ConfiguracaoEmissaoId { get; private set; }

        public ConfiguracaoEmissaoCriadaEvent(string configuracaoEmissaoId) 
            => ConfiguracaoEmissaoId = configuracaoEmissaoId;
    }
}