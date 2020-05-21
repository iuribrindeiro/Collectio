using Collectio.Domain.Base;
using Collectio.Domain.Base.ValueObjects;
using Collectio.Domain.CartaoCreditoAggregate.Events;
using System.Collections.Generic;

namespace Collectio.Domain.CartaoCreditoAggregate
{
    public class CartaoCredito : BaseOwnerEntity, IAggregateRoot
    {
        private List<Transacao> _transacoes;

        public string TenantId { get; private set; }

        public string CpfCnpjProprietario { get; private set; }

        public StatusCartaoValueObject Status { get; private set; }

        public virtual bool CartaoProcessado 
            => Status.Status == StatusCartao.Processado;
        public string Numero { get; private set; }

        public IReadOnlyCollection<Transacao> Transacoes => _transacoes;

        public CartaoCredito(string cpfCnpjProprietario, string tenantId, DadosCartaoValueObject dadosCartao)
        {
            CpfCnpjProprietario = cpfCnpjProprietario;
            TenantId = tenantId;
            _transacoes = new List<Transacao>();
            Status = StatusCartaoValueObject.Processando();
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
            Status.Processado();
            Numero = numero;
            AddEvent(new CartaoCreditoProcessadoEvent(Id.ToString(), numero));
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