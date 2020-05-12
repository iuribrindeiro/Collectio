using System;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Application.Base.Commands.Exceptions;
using Collectio.Domain.Base.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Collectio.Application.Base.Commands
{
    public class LoggingPipeline<C, R> : IPipelineBehavior<C, R>
    {
        private readonly ILogger<LoggingPipeline<C, R>> _logger;

        public LoggingPipeline(ILogger<LoggingPipeline<C, R>> logger) 
            => _logger = logger;

        public async Task<R> Handle(C request, CancellationToken cancellationToken, RequestHandlerDelegate<R> next)
        {
            var handlerType = request is ICommand ? "Command" : "Query";
            using (_logger.BeginScope($"{handlerType} {request.GetType().Name}"))
            {
                _logger.LogInformation($"Executando");
                try
                {
                    var result = await next();
                    _logger.LogInformation($"Executado com sucesso");
                    if (request is ICommand)
                        _logger.LogInformation($"Resultado: ${JsonConvert.SerializeObject(result)}");
                    return result;
                }
                catch (BusinessRulesException ex)
                {
                    _logger.LogWarning(ex, "Falha ao executar handler");
                    throw new BusinessRuleCommandException(ex.Message);
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