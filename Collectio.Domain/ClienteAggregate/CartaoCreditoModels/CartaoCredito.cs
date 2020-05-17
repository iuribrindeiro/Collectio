using System;
using System.Collections.Generic;
using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.ClienteAggregate.Events;
using Collectio.Domain.ClienteAggregate.Exceptions;

namespace Collectio.Domain.ClienteAggregate.CartaoCreditoModels
{
    public class CartaoCredito : BaseTenantEntity
    {
        private CpfCnpjValueObject _cpfCnpjProprietario;
        private Guid _clienteId;
        private Cliente _cliente;
        private StatusCartaoValueObject _status;
        private string _numero;
        private string _token;
        private List<Transacao> _transacoes;

        public Guid ClienteId => _clienteId;
        public virtual Cliente Cliente => _cliente;
        public CpfCnpjValueObject CpfCnpjProprietario => _cpfCnpjProprietario;
        public StatusCartaoValueObject Status => _status;
        public virtual bool CartaoProcessado 
            => Status.Status == StatusCartao.Processado;
        public string Token => _token;
        public string Numero => _numero;
        public IReadOnlyCollection<Transacao> Transacoes => _transacoes;

        public CartaoCredito(CpfCnpjValueObject cpfCnpjProprietario, Guid clienteId, DadosCartaoValueObject dadosCartao)
        {
            _cpfCnpjProprietario = cpfCnpjProprietario;
            _clienteId = clienteId;
            _transacoes = new List<Transacao>();
            _status = StatusCartaoValueObject.Processando();
            AddEvent(new CartaoCreditoCriadoEvent(dadosCartao, Id.ToString()));
        }

        public CartaoCredito AddTransacao(string cobrancaId, decimal valor)
        {
            if (!CartaoProcessado)
                throw new CartaoCreditoNaoProcessadoException();

            var transacao = new Transacao(cobrancaId, Id, valor);
            _transacoes.Add(transacao);
            return this;
        }

        public CartaoCredito Processado(string token, string numero)
        {
            _status.Processado();
            _token = token;
            _numero = numero;
            AddEvent(new CartaoCreditoProcessadoEvent(Id.ToString(), numero, token));
            return this;
        }

        public CartaoCredito Erro(string mensagemErro)
        {
            _status.Erro(mensagemErro);
            AddEvent(new ErroProcessarCartaoCreditoEvent(Id.ToString(), mensagemErro));
            return this;
        }
    }
}