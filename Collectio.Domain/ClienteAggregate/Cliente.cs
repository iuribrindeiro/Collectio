using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ClienteAggregate.Events;

namespace Collectio.Domain.ClienteAggregate
{
    public class Cliente : BaseOwnerEntity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string CpfCnpj { get; private set; }
        public string CartaoCreditoPadraoId { get; private set; }

        public virtual bool CartaoCreditoPadraoDefinido 
            => string.IsNullOrWhiteSpace(CartaoCreditoPadraoId);

        public Cliente(string nome, string cpfCnpj)
        {
            Nome = nome;
            CpfCnpj = cpfCnpj;
            AddEvent(new ClienteCriadoEvent(Id.ToString()));
        }

        public Cliente DefinirCartaoCreditoPadrao(string cartaoCreditoId)
        {
            var cartaoCreditoPadraoAnteriorId = CartaoCreditoPadraoId;
            CartaoCreditoPadraoId = cartaoCreditoId;
            AddEvent(new CartaoCreditoPadraoDefinidoEvent(cartaoCreditoId, cartaoCreditoPadraoAnteriorId));
            return this;
        }
    }
}

