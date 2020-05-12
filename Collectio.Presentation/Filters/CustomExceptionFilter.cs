using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Collectio.Application.Base.Commands.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Collectio.Presentation.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger) 
            => _logger = logger;

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationCommandException validationCommandException)
            {
                context.Result = new UnprocessableEntityObjectResult(new { message = validationCommandException.Message, errors = validationCommandException.CommandPropertyErrors });
            } else if (context.Exception is BusinessRuleCommandException businessRuleCommandException)
            {
                context.Result = new BadRequestObjectResult(new { message = businessRuleCommandException.Message });
            }
            else
            {
                var message = "Erro inesperado ao tentar executar a ação. Entre em contato com nosso suporte";
                context.Result = new ObjectResult(new { message }) { StatusCode = 500 };
            }
        }
    }
}
