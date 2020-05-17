using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ClienteAggregate.BoletoModels;
using Collectio.Domain.ClienteAggregate.CartaoCreditoModels;
using Collectio.Domain.ClienteAggregate.Events;
using Collectio.Domain.ClienteAggregate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Collectio.Domain.ClienteAggregate
{
    public class Cliente : BaseTenantEntity, IAggregateRoot
    {
        private string _nome;
        private List<CartaoCredito> _cartoesCredito;
        private List<Boleto> _boletos;
        private CpfCnpjValueObject _cpfCnpj;

        public string Nome => _nome;
        public virtual IReadOnlyCollection<CartaoCredito> CartoesCredito => _cartoesCredito;
        public virtual IReadOnlyCollection<Boleto> Boletos => _boletos;
        public CpfCnpjValueObject CpfCnpj => _cpfCnpj;

        public Cliente(string nome, CpfCnpjValueObject cpfCnpj)
        {
            _cartoesCredito = new List<CartaoCredito>();
            _boletos = new List<Boleto>();
            _nome = nome;
            _cpfCnpj = cpfCnpj;
            AddEvent(new ClienteCriadoEvent(this));
        }

        public CartaoCredito AddCartaoCredito(CpfCnpjValueObject cpfCnpjProprietario, DadosCartaoValueObject dadosCartao)
        {
            var cartaoCredito = new CartaoCredito(cpfCnpjProprietario, Id, dadosCartao);
            _cartoesCredito.Add(cartaoCredito);
            return cartaoCredito;
        }

        public Cliente RemoveCartaoCredito(Guid cartaoId)
        {
            var cartao = BuscarCartao(cartaoId);
            _cartoesCredito.Remove(cartao);
            AddEvent(new CartaoCreditoRemovidoEvent(Id.ToString(), cartao.ToString()));
            return this;
        }

        public Cliente AddTransacaoCartao(string cobrancaId, Guid cartaoId, decimal valor)
        {
            var cartao = BuscarCartao(cartaoId);
            cartao.AddTransacao(cobrancaId, valor);
            return this;
        }

        public Cliente AprovarTransacaoCartao(Guid transacaoCartaoId, string externalTenantId)
        {
            var transacao = BuscarTransacaoCartao(transacaoCartaoId);
            transacao.Aprovar(externalTenantId);
            return this;
        }

        public Cliente ErroTransacaoCartao(Guid transacaoCartaoId, string mensagemErro, string externalTenantId)
        {
            var transacao = BuscarTransacaoCartao(transacaoCartaoId);
            transacao.DefinirErro(mensagemErro, externalTenantId);
            return this;
        }

        public Cliente ReprocessarTransacaoCartao(Guid transacaoCartaoId, decimal valor)
        {
            var transacao = BuscarTransacaoCartao(transacaoCartaoId);
            transacao.Reprocessar(valor);
            return this;
        } 

        public Cliente AddBoleto(string cobrancaId, string formaPagamentoCobrancaId, decimal valor, 
            JurosValueObject juros = null, MultaValueObject multa = null, DescontoValueObject desconto = null)
        {
            _boletos.Add(new Boleto(Id, cobrancaId, formaPagamentoCobrancaId, valor, juros, multa, desconto));
            return this;
        }

        private CartaoCredito BuscarCartao(Guid cartaoId)
        {
            var cartao = _cartoesCredito.FirstOrDefault(c => c.Id == cartaoId);
            if (!cartao)
                throw new CartaoCreditoNaoCadastradoException();
            return cartao;
        }

        private Transacao BuscarTransacaoCartao(Guid transacaoId)
        {
            var transacao = _cartoesCredito.SelectMany(c => c.Transacoes).FirstOrDefault(t => t.Id == transacaoId);
            if (!transacao)
                throw new TransacaoCartaoNaoExisteException();
            return transacao;
        }
    }
}

