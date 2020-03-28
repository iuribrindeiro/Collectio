using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Collectio.Application.Base;
using Collectio.Application.Base.Commands;
using Collectio.Application.Base.Queries;

namespace Collectio.Presentation.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly ICommandQuerySender _commandQuerySender;

        public BaseController(ICommandQuerySender commandQuerySender)
            => _commandQuerySender = commandQuerySender;

        protected IDictionary<ErrorReason?,
            Func<CommandResponse, IActionResult>> _commandErrorParser = new Dictionary<ErrorReason?, Func<CommandResponse, IActionResult>>()
        {
            {
                ErrorReason.UnexpectedError, (result) => new ObjectResult(new {result.Message}){StatusCode = 500}
            },
            {
                ErrorReason.BusinessRulesFailure, (result) => new BadRequestObjectResult(new {result.Message, result.Errors})
            },
            {
                ErrorReason.UnprocessableEntity, (result) => new UnprocessableEntityObjectResult(new {result.Message, result.Errors})
            }
        };

        protected async Task<IActionResult> Send<R>(Command<R> command) where R : CommandResponse
        {
            var result = await _commandQuerySender.Send(command);

            if (result.IsSuccess)
                return Ok();

            return _commandErrorParser[result.ErrorReason](result);
        }

        protected async Task<IQueryable<R>> Send<R>(Query<R> query) where R : class
        {
            var result = await _commandQuerySender.Send<Query<R>, R>(query);
            return result.Results;
        }
    }
}