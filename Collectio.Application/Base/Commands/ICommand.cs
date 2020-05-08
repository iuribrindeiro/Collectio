using MediatR;

namespace Collectio.Application.Base.Commands
{
    public interface ICommand { }

    public interface ICommand<R> : IRequest<R>, ICommand
    {
    }
}