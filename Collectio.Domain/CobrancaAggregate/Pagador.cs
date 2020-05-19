using Collectio.Domain.Base;

namespace Collectio.Domain.CobrancaAggregate
{
    public class Pagador : BaseTenantEntity
    {
        private string _clienteId;
        private string? _cartaoCreditoId;

        public string ClienteId => _clienteId;
        public string? CartaoCreditoId => _cartaoCreditoId;

        public Pagador(string clienteId) 
            => _clienteId = clienteId;

        public Pagador(string clienteId, string cartaoCreditoId)
        {
            _clienteId = clienteId;
            _cartaoCreditoId = cartaoCreditoId;
        }

        public Pagador Alterar(string clienteId, string cartaoCreditoId)
        {
            _clienteId = clienteId;
            _cartaoCreditoId = cartaoCreditoId;
            return this;
        }
    }
}