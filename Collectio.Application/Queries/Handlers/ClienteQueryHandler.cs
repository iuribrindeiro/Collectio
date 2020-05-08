using Collectio.Application.Base.Queries;
using Collectio.Application.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Collectio.Domain.ClienteAggregate;

namespace Collectio.Application.Queries.Handlers
{
    public class ClienteQueryHandler : IQueryHandler<ClienteQueryRequest, IQueryable<ClienteViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IClientesRepository _clientesRepository;

        public ClienteQueryHandler(IMapper mapper, IClientesRepository clientesRepository)
        {
            _mapper = mapper;
            _clientesRepository = clientesRepository;
        }

        public async Task<IQueryable<ClienteViewModel>> Handle(ClienteQueryRequest request, CancellationToken cancellationToken)
            => _mapper.ProjectTo<ClienteViewModel>(await _clientesRepository.ListAsync());
    }
}
