using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate
{
    public class StatusConfiguracaoEmissaoValueObject : ValueObject
    {
        public StatusConfiguracaoEmissao Status { get; private set; }
        public string MensagemErro { get; private set; }

        public static StatusConfiguracaoEmissaoValueObject Processando() 
            => new StatusConfiguracaoEmissaoValueObject() { Status = StatusConfiguracaoEmissao.Processando };

        private StatusConfiguracaoEmissaoValueObject() {}

        public bool EstaProcessando
            => Status == StatusConfiguracaoEmissao.Processando;
        public bool FoiProcessado
            => Status == StatusConfiguracaoEmissao.Processado;

        public StatusConfiguracaoEmissaoValueObject Reprocessar()
        {
            if (Status == StatusConfiguracaoEmissao.Processando)
                throw new ImpossivelReprocessarConfiguracaoEmissaoProcessandoException();

            return Processando();
        }

        public StatusConfiguracaoEmissaoValueObject Processado()
        {
            if (Status != StatusConfiguracaoEmissao.Processando)
                throw new ImpossivelProcessarConfiguracaoRecebimentoException();

            Status = StatusConfiguracaoEmissao.Processado;
            return this;
        }

        public StatusConfiguracaoEmissaoValueObject Erro(string mensagemErro)
        {
            if (Status != StatusConfiguracaoEmissao.Processando)
                throw new ImpossivelDefinirErroConfiguracaoRecebimentoException();

            Status = StatusConfiguracaoEmissao.Erro;
            MensagemErro = mensagemErro;
            return this;
        }
    }
}