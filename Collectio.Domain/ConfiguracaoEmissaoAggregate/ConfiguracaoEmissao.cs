using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ConfiguracaoEmissaoAggregate.Events;
using Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate
{
    public class ConfiguracaoEmissao : BaseTenantEntity, IAggregateRoot
    {
        private AgenciaContaValueObject _agenciaConta;
        private CpfCnpjValueObject _cpfCnpj;
        private string _nomeEmpresa;
        private TelefoneValueObject _telefone;
        private EmailValueObject _email;
        private StatusConfiguracaoEmissaoValueObject _status;

        public string NomeEmpresa => _nomeEmpresa;
        public AgenciaContaValueObject AgenciaConta => _agenciaConta;
        public CpfCnpjValueObject CpfCnpj => _cpfCnpj;
        public EmailValueObject Email => _email;
        public TelefoneValueObject Telefone => _telefone;
        public StatusConfiguracaoEmissaoValueObject Status => _status;

        public ConfiguracaoEmissao(string nomeEmpresa, string agencia, string conta, string cpfCnpj, string email, string telefone, string ddd)
        {
            _nomeEmpresa = nomeEmpresa;
            _agenciaConta = new AgenciaContaValueObject(agencia, conta);
            _cpfCnpj = new CpfCnpjValueObject(cpfCnpj);
            _email = new EmailValueObject(email);
            _telefone = new TelefoneValueObject(ddd, telefone);
            _status = StatusConfiguracaoEmissaoValueObject.Processando();
            AddEvent(new ConfiguracaoEmissaoCriadaEvent(Id.ToString()));
        }

        public ConfiguracaoEmissao Alterar(string nomeEmpresa, string agencia, string conta, string cpfCnpj, string email, string telefone, string ddd)
        {
            if (_status.EstaProcessando)
                throw new ImpossivelAlterarConfiguracaoEmissaoEmProcessamentoException();

            var nomeEmpresaAnterior = _nomeEmpresa;
            var agenciaAnterior = _agenciaConta.Agencia;
            var contaAnterior = _agenciaConta.Conta;
            var cpfCnpjAnterior = _cpfCnpj.Value;
            var emailAnterior = _email.Value;
            var telefoneAnterior = _telefone.Telefone;
            var dddAnterior = _telefone.Ddd;

            var configuracaoEmissaoMudou = ConfiguracaoEmissaoMudou(nomeEmpresa, agencia, conta, cpfCnpj, email, telefone, ddd, nomeEmpresaAnterior, 
                agenciaAnterior, contaAnterior, cpfCnpjAnterior, emailAnterior, telefoneAnterior, dddAnterior);

            if (configuracaoEmissaoMudou)
                Reprocessar();

            _nomeEmpresa = nomeEmpresa;
            _agenciaConta = new AgenciaContaValueObject(agencia, conta);
            _cpfCnpj = new CpfCnpjValueObject(cpfCnpj);
            _email = new EmailValueObject(email);
            _telefone = new TelefoneValueObject(ddd, telefone);

            AddEvent(new ConfiguracaoEmissaoAlteradaEvent(Id.ToString(), nomeEmpresaAnterior, agenciaAnterior, cpfCnpjAnterior, contaAnterior, emailAnterior, telefoneAnterior, dddAnterior));
            return this;
        }

        public ConfiguracaoEmissao Reprocessar()
        {
            _status = _status.Reprocessar();
            AddEvent(new ConfiguracaoEmissaoReprocessandoEvent(Id.ToString()));
            return this;
        }

        public ConfiguracaoEmissao FinalizarProcessamento()
        {
            _status.Processado();
            AddEvent(new ConfiguracaoEmissaoProcessadaEvent(Id.ToString()));
            return this;
        }

        public ConfiguracaoEmissao ErroProcessamento(string mensagemErro)
        {
            _status.Erro(mensagemErro);
            AddEvent(new ErroProcessarConfiguracaoEmissaoEvent(Id.ToString()));
            return this;
        }

        private bool ConfiguracaoEmissaoMudou(string nomeEmpresa, string agencia, string conta, string cpfCnpj, string email,
            string telefone, string ddd, string nomeEmpresaAnterior, string agenciaAnterior, string contaAnterior,
            string cpfCnpjAnterior, string emailAnterior, string telefoneAnterior, string dddAnterior)
            => nomeEmpresaAnterior != nomeEmpresa || agencia != agenciaAnterior || conta != contaAnterior ||
               cpfCnpj != cpfCnpjAnterior || email != emailAnterior || telefone != telefoneAnterior || ddd != dddAnterior;
    }
}
