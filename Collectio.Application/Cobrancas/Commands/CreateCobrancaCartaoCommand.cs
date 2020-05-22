using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Collectio.Application.Base.Commands;
using Collectio.Application.Cobrancas.CommandValidators;
using Collectio.Application.Cobrancas.ViewModels;
using Collectio.Application.Profiles;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CobrancaAggregate;
using Collectio.Domain.ConfiguracaoEmissaoAggregate;
using CartaoCredito = Collectio.Domain.CobrancaAggregate.CartaoCredito;

namespace Collectio.Application.Cobrancas.Commands
{
    public class CreateCobrancaCartaoCommand : BaseCreateCobrancaCommand, ICommand<string>, IMapping
    {
        public CartaoCreditoCobrancaViewModel CartaoCredito { get; set; }

        public void Mapping(Profile profile) 
            => profile.CreateMap<CreateCobrancaCartaoCommand, Cobranca>()
                .ConstructUsing((c, context) => Cobranca.Cartao(
                    c.Valor, c.Vencimento, c.ConfiguracaoEmissorId,
                    c.Cliente.Nome, c.Cliente.CpfCnpj, c.Cliente.Email,
                    context.Mapper.Map<Telefone>(c.Cliente.Telefone), context.Mapper.Map<CartaoCredito>(c.CartaoCredito), 
                    context.Mapper.Map<Endereco>(c.Cliente.Endereco), c.Cliente.TenantId));
    }

    public class CreateCobrancaCartaoCommandHandler : ICommandHandler<CreateCobrancaCartaoCommand, string>
    {
        private readonly ICobrancasRepository _cobrancasRepository;
        private readonly IMapper _mapper;

        public CreateCobrancaCartaoCommandHandler(ICobrancasRepository cobrancasRepository, IMapper mapper)
        {
            _cobrancasRepository = cobrancasRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateCobrancaCartaoCommand request, CancellationToken cancellationToken)
        {
            var cobranca = _mapper.Map<Cobranca>(request);
            await _cobrancasRepository.SaveAsync(cobranca);
            return cobranca.Id.ToString();
        }
    }

    public class CreateCobrancaCartaoCommandValidator : BaseCreateCobrancaCommandValidator<CreateCobrancaCartaoCommand>
    {
        public CreateCobrancaCartaoCommandValidator(IConfiguracaoEmissaoRepository configuracaoEmissaoRepository) 
            : base(configuracaoEmissaoRepository)
        {
            RuleFor(c => c.CartaoCredito);
        }
    }
}