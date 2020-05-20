using Collectio.Application.Cobrancas.Commands;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Collectio.Presentation.Controllers
{
    public class CobrancaController : BaseController
    {
        [HttpPost]
        public Task<IActionResult> Cartao([FromBody] CreateCobrancaCartaoCommand command) 
            => Send(command);

        [HttpPost]
        public Task<IActionResult> Boleto([FromBody] CreateCobrancaBoletoCommand command)
            => Send(command);
    }
}
