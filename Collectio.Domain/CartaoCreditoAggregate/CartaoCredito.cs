using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CartaoCreditoAggregate.Events;
using System.Collections.Generic;

namespace Collectio.Domain.CartaoCreditoAggregate
{
    public class CartaoCredito : BaseTenantEntity, IAggregateRoot
    {
        private CpfCnpjValueObject _cpfCnpjProprietario;
        private string _clienteId;
        private StatusCartaoValueObject _status;
        private string _numero;
        private List<Transacao> _transacoes;

        public string ClienteId => _clienteId;
        public CpfCnpjValueObject CpfCnpjProprietario => _cpfCnpjProprietario;
        public StatusCartaoValueObject Status => _status;
        public virtual bool CartaoProcessado 
            => Status.Status == StatusCartao.Processado;
        public string Numero => _numero;
        public IReadOnlyCollection<Transacao> Transacoes => _transacoes;

        public CartaoCredito(CpfCnpjValueObject cpfCnpjProprietario, string clienteId, DadosCartaoValueObject dadosCartao)
        {
            _cpfCnpjProprietario = cpfCnpjProprietario;
            _clienteId = clienteId;
            _transacoes = new List<Transacao>();
            _status = StatusCartaoValueObject.Processando();
            AddEvent(new CartaoCreditoCriadoEvent(dadosCartao, Id.ToString()));
        }

        public CartaoCredito AddTransacao(string cobrancaId, string contaBancariaId, decimal valor)
        {
            var transacao = new Transacao(cobrancaId, contaBancariaId, this, valor);
            _transacoes.Add(transacao);
            return this;
        }

        public CartaoCredito Processado(string numero)
        {
            _status.Processado();
            _numero = numero;
            AddEvent(new CartaoCreditoProcessadoEvent(Id.ToString(), numero));
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