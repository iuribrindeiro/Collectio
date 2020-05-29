using System;
using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CobrancaAggregate.Exceptions;

namespace Collectio.Domain.CobrancaAggregate
{
    public class ClienteCobranca : BaseOwnerEntity
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string CpfCnpj { get; private set; }
        public virtual Endereco Endereco { get; private set; }
        public virtual CartaoCreditoCobranca CartaoCreditoCobranca { get; private set; }
        public virtual Telefone Telefone { get; private set; }
        public string TenantId { get; private set; }
        public virtual Cobranca Cobranca { get; private set; }
        public Guid CobrancaId { get; private set; }

        private ClienteCobranca() {}

        public ClienteCobranca(Cobranca cobranca, string nome, string cpfCnpj, 
            string email, string tenantId, Telefone telefone, 
            Endereco endereco, CartaoCreditoCobranca cartaoCreditoCobranca)
        {
            TenantId = tenantId;
            Nome = nome;
            CpfCnpj = cpfCnpj;
            Email = email;
            Telefone = telefone;
            Cobranca = cobranca;
            CartaoCreditoCobranca = cartaoCreditoCobranca;
            Endereco = endereco;

            ValidaDadosClienteEmissaoCartao(cartaoCreditoCobranca);
            ValidaDadosClienteEmissaoBoleto();
        }

        public ClienteCobranca AlterarCartaoCredito(CartaoCreditoCobranca cartaoCreditoCobranca)
        {
            ValidaAlteracaoCliente();

            if (Cobranca.FormaPagamentoBoleto)
                throw new CobrancaBoletoNaoDeveConterCartaoNoClienteException();

            CartaoCreditoCobranca = cartaoCreditoCobranca;
            return this;
        }

        public ClienteCobranca Alterar(string tenantId, string nome, string cpfCnpj, string email, Telefone telefone, Endereco endereco)
        {
            ValidaAlteracaoCliente();
            ValidaDadosClienteEmissaoBoleto();

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

        private void ValidaDadosClienteEmissaoBoleto()
        {
            if (CartaoCreditoCobranca && Cobranca.FormaPagamentoBoleto)
                throw new CobrancaBoletoNaoDeveConterCartaoNoClienteException();
        }

        private void ValidaDadosClienteEmissaoCartao(CartaoCreditoCobranca cartaoCreditoCobranca)
        {
            if (!cartaoCreditoCobranca && Cobranca.FormaPagamentoCartao)
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
}