using MediatR;

namespace Collectio.Application.Base.Commands
{
    public interface ICommand<R> : IRequest<R>
    {
    }
}