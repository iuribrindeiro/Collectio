using System.Linq;
using Collectio.Domain.Base;
using MediatR;

namespace Collectio.Application.Base.Commands
{
    public interface ICommand { }

    public interface ICommand<R> : IRequest<R>, ICommand
    {
    }

    public interface ICommandHandler<C, R> : IRequestHandler<C, R>
        where C : ICommand<R>
    {
    }
}