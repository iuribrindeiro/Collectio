using System.Threading.Tasks;
using Collectio.Application.ConfiguracoesEmissao.Commands;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Collectio.Presentation.Controllers
{
    public class ConfiguracaoEmissaoController : BaseController
    {
        [HttpPost]
        public Task<IActionResult> Post([FromBody] CreateConfiguracaoEmissaoCommand command)
            => Send(command);
    }
}
