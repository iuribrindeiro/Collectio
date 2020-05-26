using Collectio.Application.Cobrancas.Commands;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Collectio.Presentation.Controllers
{
    [Authorize]
    public class CobrancasController : BaseController
    {
        [HttpPost("cartao")]
        public async Task<IActionResult> Cartao([FromBody] CreateCobrancaCartaoCommand command)
        {
            var result = await Send(command);
            return Created("created", result);
        }

        [HttpPost]
        public async Task<IActionResult> Boleto([FromBody] CreateCobrancaBoletoCommand command)
        {
            var result = await Send(command);
            return Created("created", result);
        }
    }
}
