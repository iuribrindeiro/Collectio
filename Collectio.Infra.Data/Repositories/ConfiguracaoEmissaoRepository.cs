using System.Linq;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using Collectio.Infra.Data.Repositories.Base;

namespace Collectio.Infra.Data.Repositories
{
    public class ConfiguracaoEmissaoRepository : BaseRepository<ConfiguracaoEmissao>, IConfiguracaoEmissaoRepository
    {
        public ConfiguracaoEmissaoRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }
    }
}