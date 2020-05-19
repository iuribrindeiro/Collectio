using Collectio.Application.Clientes.Commands;
using Collectio.Application.Queries;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Collectio.Presentation.Controllers
{
    public class ClientesController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Post(CreateClienteCommand createClienteCommand) 
            => await Send(createClienteCommand);

        [HttpGet, EnableQuery]
        public async Task<IActionResult> Get([FromQuery]ClienteQueryRequest query)
            => await Send(query);
    }
}
