using MediatR;

namespace Collectio.Application.Base.Commands
{
    public abstract class Command<R> : IRequest<R> where R : CommandResponse
    {
    }
}