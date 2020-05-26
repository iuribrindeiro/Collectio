using Collectio.Application.Base;
using Collectio.Application.Base.Commands;
using Collectio.Application.Base.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Collectio.Application.Base.Commands.Exceptions;

namespace Collectio.Presentation.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private ICommandQuerySender _commandQuerySender 
            => HttpContext.RequestServices.GetService<ICommandQuerySender>();

        protected async Task<R> Send<R>(ICommand<R> command)
        {
            if (command is null)
                throw new ValidationCommandException();

            return await _commandQuerySender.Send(command);
        }

        protected async Task<R> Send<R>(IQuery<R> query) where R : class
        {
            if (query is null)
                throw new ValidationCommandException();

            return await _commandQuerySender.Send<IQuery<R>, R>(query);
        }
    }
}