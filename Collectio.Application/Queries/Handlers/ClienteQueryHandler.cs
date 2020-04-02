using Collectio.Application.Base.Queries;
using Collectio.Application.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Collectio.Domain.ClienteAggregate;

namespace Collectio.Application.Queries.Handlers
{
    public class ClienteQueryRequest : Query<ClienteViewModel>
    {}

    public class ClienteQueryHandler : AbstractQueryHandler<ClienteQueryRequest, ClienteViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IClientesRepository _clientesRepository;

        public ClienteQueryHandler(IMapper mapper, IClientesRepository clientesRepository, ILogger<AbstractQueryHandler<ClienteQueryRequest, ClienteViewModel>> logger) : base(logger)
        {
            _mapper = mapper;
            _clientesRepository = clientesRepository;
        }

        protected override async Task<IQueryable<ClienteViewModel>> HandleAsync(ClienteQueryRequest query) 
            => _mapper.ProjectTo<ClienteViewModel>(await _clientesRepository.ListAsync());
    }
}
