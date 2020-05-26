using Collectio.Application.Clientes.Commands;
using Collectio.Application.Queries;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Collectio.Presentation.Controllers
{
    [Authorize]
    public class ClientesController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Post(CreateClienteCommand createClienteCommand)
        {
            var result = await Send(createClienteCommand);
            return Created("Created", result);
        }

        [HttpGet, EnableQuery]
        public async Task<IActionResult> Get([FromQuery]ClienteQueryRequest query) 
            => Ok(await Send(query));
    }
}
