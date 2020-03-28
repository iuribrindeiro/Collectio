using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Collectio.Application.Base.Queries
{
    public abstract class AbstractQueryHandler<Q, R> : IRequestHandler<Q, QueryResult<R>>
        where Q : Query<R>
        where R : class
    {
        private readonly ILogger<AbstractQueryHandler<Q, R>> _logger;

        public AbstractQueryHandler(ILogger<AbstractQueryHandler<Q, R>> logger) 
            => _logger = logger;

        public async Task<QueryResult<R>> Handle(Q request, CancellationToken cancellationToken)
        {
            using (_logger.BeginScope($"{request.GetType().Name}"))
            {
                try
                {
                    var results = await HandleAsync(request);
                    _logger.LogInformation($"{GetType().Name} executado com sucesso");
                    return QueryResult<R>.Success(results);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, $"Erro ao executar query handler {GetType().Name}");
                    return QueryResult<R>.Failed();
                }
            }
        }

        protected abstract Task<IQueryable<R>> HandleAsync(Q query);
    }
}