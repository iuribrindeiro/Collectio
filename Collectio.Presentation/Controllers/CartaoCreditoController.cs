using System.Threading.Tasks;
using Collectio.Application.CartoesCredito.Commands;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Collectio.Presentation.Controllers
{
    public class CartaoCreditoController : BaseController
    {
        [HttpPost]
        public Task<IActionResult> Post([FromBody] CreateCartaoCreditoCommand command) 
            => Send(command);
    }
}