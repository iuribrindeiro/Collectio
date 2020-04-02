using Collectio.Domain.Base.Exceptions;
using Collectio.Infra.CrossCutting.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Collectio.Application.Base.Commands
{
    public abstract class AbstractCommandHandler<T, R> : IRequestHandler<T, R> where T : Command<R> where R : CommandResponse, new()
    {
        protected readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AbstractCommandHandler(ILogger logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<R> Handle(T request, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope($"{GetType().Name}"))
            {
                try
                {
                    _logger.LogInformation($"Executando");
                    var result = await HandleAsync(request);
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInformation($"Executado com sucesso");
                    return result;
                }
                catch (UnprocessableEntityException e)
                {
                    _logger.LogError(e, e.Message);
                    return CommandResponse.UnprocessableEntity<R>(e.Message, e.Errors);
                }
                catch (BusinessRulesException e)
                {
                    _logger.LogError(e, e.Message);
                    return CommandResponse.BusinessRulesFailure<R>(e.Message, e.Errors);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, $"Erro ao executar command handler");
                    return CommandResponse.UnexpectedError<R>("Erro inesperado ao tentar executar a ação. Entre em contato com nosso suporte");
                }
            }
        }

        protected abstract Task<R> HandleAsync(T command);
    }
}