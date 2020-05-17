using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Collectio.Domain.Base;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.CobrancaAggregate.Events;

namespace Collectio.Domain.ClienteAggregate.EventHandlers
{
    public class CobrancaCriacaEventHandler : IDomainEventHandler<CobrancaCriadaEvent>
    {
        private readonly ICobrancasRepository _cobrancasRepository;
        private readonly IClientesRepository _clientesRepository;

        public CobrancaCriacaEventHandler(ICobrancasRepository cobrancasRepository, IClientesRepository clientesRepository)
        {
            _cobrancasRepository = cobrancasRepository;
            _clientesRepository = clientesRepository;
        }

        public async Task Handle(CobrancaCriadaEvent domainEvent, CancellationToken cancellationToken)
        {
            var cobranca = await _cobrancasRepository.FindAsync(Guid.Parse(domainEvent.CobrancaId));
            if (cobranca.FormaPagamentoCartao)
            {
                var cliente = await _clientesRepository.FindAsync(Guid.Parse(cobranca.PagadorId));
                var cartao = cliente.CartoesCredito.FirstOrDefault(c => c.Id == Guid.Parse(cobranca.FormaPagamento.Id));
                cliente.AddTransacaoCartao(cobranca.Id.ToString(), Guid.Parse(cobranca.FormaPagamento.Id), cobranca.Valor);
            }
        }
    }
}
