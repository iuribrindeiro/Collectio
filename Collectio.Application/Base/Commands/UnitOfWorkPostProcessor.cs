using System.Threading;
using System.Threading.Tasks;
using Collectio.Infra.CrossCutting.Services.Interfaces;
using MediatR.Pipeline;

namespace Collectio.Application.Base.Commands
{
    public class UnitOfWorkPostProcessor<C, R> : IRequestPostProcessor<C, R>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkPostProcessor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Process(C request, R response, CancellationToken cancellationToken) 
            => await _unitOfWork.SaveChangesAsync();
    }
}