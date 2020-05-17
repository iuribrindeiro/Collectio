using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ClienteAggregate.Events;

namespace Collectio.Domain.ClienteAggregate
{
    public class Cliente : BaseTenantEntity, IAggregateRoot
    {
        private string _nome;
        private CpfCnpjValueObject _cpfCnpj;
        private string _cartaoCreditoPadraoId;

        public string Nome => _nome;
        public CpfCnpjValueObject CpfCnpj => _cpfCnpj;
        public string CartaoCreditoPadraoId => _cartaoCreditoPadraoId;

        public Cliente(string nome, CpfCnpjValueObject cpfCnpj)
        {
            _nome = nome;
            _cpfCnpj = cpfCnpj;
            AddEvent(new ClienteCriadoEvent(Id.ToString()));
        }

        public Cliente DefinirCartaoCreditoPadrao(string cartaoCreditoId)
        {
            var cartaoCreditoPadraoAnteriorId = _cartaoCreditoPadraoId;
            _cartaoCreditoPadraoId = cartaoCreditoId;
            AddEvent(new CartaoCreditoPadraoDefinidoEvent(cartaoCreditoId, cartaoCreditoPadraoAnteriorId));
            return this;
        }
    }
}

