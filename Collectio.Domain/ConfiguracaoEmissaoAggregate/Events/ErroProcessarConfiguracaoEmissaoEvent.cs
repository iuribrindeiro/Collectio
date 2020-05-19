using Collectio.Domain.Base;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate.Events
{
    public class ErroProcessarConfiguracaoEmissaoEvent : IDomainEvent
    {
        public string ConfiguracaoEmissaoId { get; private set; }

        public ErroProcessarConfiguracaoEmissaoEvent(string configuracaoEmissaoId) 
            => ConfiguracaoEmissaoId = configuracaoEmissaoId;
    }
}