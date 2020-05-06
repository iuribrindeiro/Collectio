using MediatR;

namespace Collectio.Application.Base.Commands
{
    public interface ICommandHandler<C, R> : IRequestHandler<C, R>
        where C : ICommand<R>
    {}
}