using System;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.Base.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Collectio.Application.Base.Commands
{
    public class LoggingPipeline<C, R> : IPipelineBehavior<C, R>
        where C : ICommand<R>
    {
        private readonly ILogger<LoggingPipeline<C, R>> _logger;

        public LoggingPipeline(ILogger<LoggingPipeline<C, R>> logger) 
            => _logger = logger;

        public async Task<R> Handle(C request, CancellationToken cancellationToken, RequestHandlerDelegate<R> next)
        {
            using (_logger.BeginScope($"{request.GetType().Name}"))
            {
                _logger.LogInformation($"Executando");
                try
                {
                    var result = await next();
                    _logger.LogInformation($"Executado com sucesso com resultado: {JsonConvert.SerializeObject(result)}");
                    return result;
                }
                catch (MultipleErrorsException ex)
                {
                    _logger.LogWarning(ex, "Falha ao executar handler");
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Erro critico ao executar handler");
                    throw;
                }
            }
        }
    }
}