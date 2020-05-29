using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ConfiguracaoEmissaoAggregate.Events;
using Collectio.Domain.ConfiguracaoEmissaoAggregate.Exceptions;

namespace Collectio.Domain.ConfiguracaoEmissaoAggregate
{
    public class ConfiguracaoEmissao : BaseOwnerEntity, IAggregateRoot
    {
        public string NomeEmpresa { get; private set; }
        public string CpfCnpj { get; private set; }
        public string Email { get; private set; }
        public virtual Telefone Telefone { get; private set; }
        public virtual AgenciaConta AgenciaConta { get; private set; }
        public virtual StatusConfiguracaoEmissaoValueObject Status { get; private set; }

        private ConfiguracaoEmissao() {}

        public ConfiguracaoEmissao(string nomeEmpresa, string cpfCnpj, string email, AgenciaConta agenciaConta, Telefone telefone)
        {
            NomeEmpresa = nomeEmpresa;
            AgenciaConta = agenciaConta;
            CpfCnpj = cpfCnpj;
            Email = email;
            Telefone = telefone;
            Status = StatusConfiguracaoEmissaoValueObject.Processando();
            AddEvent(new ConfiguracaoEmissaoCriadaEvent(Id.ToString()));
        }

        public ConfiguracaoEmissao Alterar(string nomeEmpresa, string cpfCnpj, string email, AgenciaConta agenciaConta, Telefone telefone)
        {
            if (Status.EstaProcessando)
                throw new ImpossivelAlterarConfiguracaoEmissaoEmProcessamentoException();

            var nomeEmpresaAnterior = NomeEmpresa;
            var agenciaAnterior = AgenciaConta.Agencia;
            var contaAnterior = AgenciaConta.Conta;
            var cpfCnpjAnterior = CpfCnpj;
            var emailAnterior = Email;
            var telefoneAnterior = Telefone.Numero;
            var dddAnterior = Telefone.Ddd;
            

            NomeEmpresa = nomeEmpresa;
            AgenciaConta = agenciaConta;
            CpfCnpj = cpfCnpj;
            Email = email;
            Telefone = telefone;

            Reprocessar();
            AddEvent(new ConfiguracaoEmissaoAlteradaEvent(Id.ToString(), nomeEmpresaAnterior, agenciaAnterior, cpfCnpjAnterior, contaAnterior, emailAnterior, telefoneAnterior, dddAnterior));
            return this;
        }

        public ConfiguracaoEmissao Reprocessar()
        {
            Status = Status.Reprocessar();
            AddEvent(new ConfiguracaoEmissaoReprocessandoEvent(Id.ToString()));
            return this;
        }

        public ConfiguracaoEmissao FinalizarProcessamento()
        {
            Status.Processado();
            AddEvent(new ConfiguracaoEmissaoProcessadaEvent(Id.ToString()));
            return this;
        }

        public ConfiguracaoEmissao ErroProcessamento(string mensagemErro)
        {
            Status.Erro(mensagemErro);
            AddEvent(new ErroProcessarConfiguracaoEmissaoEvent(Id.ToString()));
            return this;
        }
    }
}
