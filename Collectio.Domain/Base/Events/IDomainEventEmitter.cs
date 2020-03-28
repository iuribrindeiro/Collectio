using System.Threading.Tasks;

namespace Collectio.Domain.Base
{
    public interface IDomainEventEmitter
    {
        Task PublishAsync(IDomainEvent domainEvent);
        Task PublishAsync(IDomainEvent[] domainEvents);
    }
}