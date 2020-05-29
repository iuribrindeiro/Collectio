using System;
using System.Threading.Tasks;
using Collectio.Application.CartoesCredito.Commands;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectio.Presentation.Controllers
{
    [Authorize]
    [Route("api/cartoes-credito")]
    public class CartoesCreditoController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCartaoCreditoCommand command)
        {
            var result = await Send(command);
            return Created("created", result);
        }
    }
}