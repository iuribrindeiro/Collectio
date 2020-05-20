using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using System.Threading;
using System.Threading.Tasks;

namespace Collectio.Application.ConfiguracoesEmissao.Commands
{
    public class CreateConfiguracaoEmissaoCommandHandler : ICommandHandler<CreateConfiguracaoEmissaoCommand, string>
    {
        private readonly IConfiguracaoEmissaoRepository _configuracaoEmissaoRepository;
        private readonly IMapper _mapper;

        public CreateConfiguracaoEmissaoCommandHandler(IConfiguracaoEmissaoRepository configuracaoEmissaoRepository, IMapper mapper)
        {
            _configuracaoEmissaoRepository = configuracaoEmissaoRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateConfiguracaoEmissaoCommand request, CancellationToken cancellationToken)
        {
            var configuracaoEmissao = _mapper.Map<ConfiguracaoEmissao>(request);
            await _configuracaoEmissaoRepository.SaveAsync(configuracaoEmissao);
            return configuracaoEmissao.Id.ToString();
        }
    }
}