using Microsoft.AspNetCore.Mvc.Filters;
using System;
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
            _logger.LogCritical(context.Exception, "Exceção não tratada");
            var message = "Erro inesperado ao tentar executar a ação. Entre em contato com nosso suporte";
            context.Result = new ObjectResult(new { message }) { StatusCode = 500 };
        }
    }
}
