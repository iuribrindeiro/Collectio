using Collectio.Domain.Base;

namespace Collectio.Domain.ClienteAggregate.Events
{
    public class BoletoCriadoEvent : IDomainEvent
    {
        private string _boletoId;

        public string BoletoId => _boletoId;

        public BoletoCriadoEvent(string boletoId) 
            => _boletoId = boletoId;
    }
}