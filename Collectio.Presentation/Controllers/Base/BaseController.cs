using Collectio.Application.Base;
using Collectio.Application.Base.Commands;
using Collectio.Application.Base.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Collectio.Presentation.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private ICommandQuerySender _commandQuerySender 
            => HttpContext.RequestServices.GetService<ICommandQuerySender>();

        protected async Task<IActionResult> Send<R>(ICommand<R> command)
        {
            var result = await _commandQuerySender.Send(command);
            return Ok(result);
        }

        protected async Task<IActionResult> Send<R>(IQuery<R> query) where R : class
        {
            var result = await _commandQuerySender.Send<IQuery<R>, R>(query);
            return Ok(result);
        }
    }
}