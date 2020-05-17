using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Collectio.Domain.Base
{
    public interface IDomainEventEmitter
    {
        Task PublishAsync(IDomainEvent domainEvent);
        Task PublishAsync(IDomainEvent[] domainEvents);
    }

    public interface IDomainEventHandler<T> : INotificationHandler<T> where T : IDomainEvent
    {
    }
}