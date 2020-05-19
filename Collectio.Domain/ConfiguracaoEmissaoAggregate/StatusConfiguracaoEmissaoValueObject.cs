using Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate
{
    public class StatusConfiguracaoEmissaoValueObject
    {
        private StatusConfiguracaoEmissao _status;
        private string _mensagemErro;

        public StatusConfiguracaoEmissao Status => _status;
        public string MensagemErro => _mensagemErro;

        public static StatusConfiguracaoEmissaoValueObject Processando() 
            => new StatusConfiguracaoEmissaoValueObject();

        private StatusConfiguracaoEmissaoValueObject() 
            => _status = StatusConfiguracaoEmissao.Processando;

        public bool EstaProcessando
            => Status == StatusConfiguracaoEmissao.Processando;
        public bool FoiProcessado
            => Status == StatusConfiguracaoEmissao.Processado;

        public StatusConfiguracaoEmissaoValueObject Reprocessar()
        {
            if (Status == StatusConfiguracaoEmissao.Processando)
                throw new ImpossivelReprocessarConfiguracaoRecebimentoProcessandoException();

            return Processando();
        }

        public StatusConfiguracaoEmissaoValueObject Processado()
        {
            if (Status != StatusConfiguracaoEmissao.Processando)
                throw new ImpossivelProcessarConfiguracaoRecebimentoException();

            _status = StatusConfiguracaoEmissao.Processado;
            return this;
        }

        public StatusConfiguracaoEmissaoValueObject Erro(string mensagemErro)
        {
            if (Status != StatusConfiguracaoEmissao.Processando)
                throw new ImpossivelDefinirErroConfiguracaoRecebimentoException();

            _status = StatusConfiguracaoEmissao.Erro;
            _mensagemErro = mensagemErro;
            return this;
        }
    }
}