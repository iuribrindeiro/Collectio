﻿using Collectio.Application.Base;
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