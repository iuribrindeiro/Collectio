﻿using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CartaoCreditoAggregate.Events;
using System.Collections.Generic;

namespace Collectio.Domain.CartaoCreditoAggregate
{
    public class CartaoCredito : BaseOwnerEntity, IAggregateRoot
    {
        private List<Transacao> _transacoes;

        public string CpfCnpjProprietario { get; private set; }

        public virtual StatusCartaoValueObject Status { get; private set; }

        public string Numero { get; private set; }

        public string Nome { get; private set; }

        public virtual bool ProcessamentoFinalizado
            => Status.Status == StatusCartao.Processado;

        public IReadOnlyCollection<Transacao> Transacoes => _transacoes;


        private CartaoCredito() {}

        public CartaoCredito(string cpfCnpjProprietario, DadosCartaoValueObject dadosCartao)
        {
            CpfCnpjProprietario = cpfCnpjProprietario;
            _transacoes = new List<Transacao>();
            Nome = dadosCartao.NomeProprietario;
            Numero = $"{dadosCartao.Numero.Substring(0, 4)}********{dadosCartao.Numero.Substring(12, 4)}";
            Status = StatusCartaoValueObject.Processando();
            AddEvent(new CartaoCreditoCriadoEvent(dadosCartao, Id.ToString()));
        }

        public CartaoCredito AddTransacao(string cobrancaId, decimal valor)
        {
            var transacao = new Transacao(cobrancaId, this, valor);
            _transacoes.Add(transacao);
            return this;
        }

        public CartaoCredito Processado()
        {
            Status.Processado();
            AddEvent(new CartaoCreditoProcessadoEvent(Id.ToString()));
            return this;
        }

        public CartaoCredito Erro(string mensagemErro)
        {
            Status.Erro(mensagemErro);
            AddEvent(new ErroProcessarCartaoCreditoEvent(Id.ToString(), mensagemErro));
            return this;
        }
    }
}