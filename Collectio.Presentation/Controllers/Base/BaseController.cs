using Collectio.Application.Base;
using Collectio.Application.Base.Commands;
using Collectio.Application.Base.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectio.Domain.Base.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Collectio.Presentation.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly ICommandQuerySender _commandQuerySender;

        public BaseController(ICommandQuerySender commandQuerySender)
            => _commandQuerySender = commandQuerySender;

        protected async Task<IActionResult> Send<R>(ICommand<R> command)
        {
            try
            {   
                var result = await _commandQuerySender.Send(command);
                return Ok(result);
            }
            catch (BusinessRulesException e)
            {
                return new BadRequestObjectResult(new {e.Message, e.Errors});
            }
            catch (UnprocessableEntityException e)
            {
                return new UnprocessableEntityObjectResult(new {e.Message, e.Errors});
            }
        }

        protected async Task<IActionResult> Send<R>(Query<R> query) where R : class
        {
            var result = await _commandQuerySender.Send<Query<R>, R>(query);
            return Ok(result.Results);
        }
    }
}