﻿using System;
using Collectio.Application.ConfiguracoesEmissao.Commands;
using Collectio.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Collectio.Presentation.Controllers
{
    [Route("api/configuracoes-emissao")]
    [Authorize]
    public class ConfiguracoesEmissaoController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody, Required] CreateConfiguracaoEmissaoCommand command)
        {
            var result = await Send(command);
            return Created("created", result);
        }
    }
}
