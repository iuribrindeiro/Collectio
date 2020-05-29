using Collectio.Domain.Base;

namespace Collectio.Domain.CartaoCreditoAggregate.Events
{
    public class CartaoCreditoProcessadoEvent : IDomainEvent
    {
        public string CartaoId { get; private set; }

        public CartaoCreditoProcessadoEvent(string cartaoId)
        {
            CartaoId = cartaoId;
        }
    }
}