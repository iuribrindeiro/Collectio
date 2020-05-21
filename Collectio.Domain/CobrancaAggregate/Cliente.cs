using System;
using Collectio.Domain.Base;
using Collectio.Domain.Base.Exceptions;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CobrancaAggregate.Exceptions;

namespace Collectio.Domain.CobrancaAggregate
{
    public class Cliente : BaseOwnerEntity
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string CpfCnpj { get; private set; }
        public virtual Endereco Endereco { get; private set; }
        public virtual CartaoCredito CartaoCredito { get; private set; }
        public virtual Telefone Telefone { get; private set; }
        public string TenantId { get; private set; }
        public virtual Cobranca Cobranca { get; private set; }

        private Cliente() {}

        public Cliente(Cobranca cobranca, string nome, string cpfCnpj, 
            string email, string tenantId, Telefone telefone, 
            Endereco endereco, CartaoCredito cartaoCredito)
        {
            TenantId = tenantId;
            Nome = nome;
            CpfCnpj = cpfCnpj;
            Email = email;
            Telefone = telefone;
            Cobranca = cobranca;
            CartaoCredito = cartaoCredito;
            Endereco = endereco;

            ValidaDadosClienteEmissaoCartao(cartaoCredito);
            ValidaDadosClienteEmissaoBoleto(endereco);
        }

        public Cliente AlterarCartaoCredito(CartaoCredito cartaoCredito)
        {
            ValidaAlteracaoCliente();

            if (Cobranca.FormaPagamentoBoleto)
                throw new CobrancaBoletoNaoDeveConterCartaoNoClienteException();

            CartaoCredito = cartaoCredito;
            return this;
        }

        public Cliente Alterar(string tenantId, string nome, string cpfCnpj, string email, Telefone telefone, Endereco endereco)
        {
            ValidaAlteracaoCliente();
            ValidaDadosClienteEmissaoBoleto(endereco);

            TenantId = tenantId;
            Nome = nome;
            CpfCnpj = cpfCnpj;
            Email = email;
            Telefone = telefone;
            Endereco = endereco;
            return this;
        }

        public override string ToString()
            => Nome;

        private void ValidaDadosClienteEmissaoBoleto(Endereco endereco)
        {
            if (!endereco && Cobranca.FormaPagamentoBoleto)
                throw new CobrancaBoletoDeveConterEnderecoClienteVinculadoException();

            if (CartaoCredito && Cobranca.FormaPagamentoBoleto)
                throw new CobrancaBoletoNaoDeveConterCartaoNoClienteException();
        }

        private void ValidaDadosClienteEmissaoCartao(CartaoCredito cartaoCredito)
        {
            if (!cartaoCredito && Cobranca.FormaPagamentoCartao)
                throw new CobrancasComCartaoDevemPossuirClienteComCartaoCreditoVinculadoException(Cobranca, this);
        }

        private void ValidaAlteracaoCliente()
        {
            if (Cobranca.Transacao.ProcessamentoPendente)
                throw new ImpossivelAlterarDadosClienteQuandoCobrancaEstaEmProcessamentoException();

            if (Cobranca.Status == StatusCobranca.Pago)
                throw new ImpossivelAlterarDadosClienteQuandoCobrancaJaEstaPagaException();
        }
    }

    public class ImpossivelAlterarDadosClienteQuandoCobrancaJaEstaPagaException : BusinessRulesException
    {
        public ImpossivelAlterarDadosClienteQuandoCobrancaJaEstaPagaException() : base("Não é possível atualizar os dados de um cliente cuja cobrança já está paga")
        {
        }
    }
}