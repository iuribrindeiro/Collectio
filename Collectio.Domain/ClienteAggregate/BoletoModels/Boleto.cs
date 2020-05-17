using Collectio.Domain.Base;
using System;
using Collectio.Domain.ClienteAggregate.Events;

namespace Collectio.Domain.ClienteAggregate.BoletoModels
{
    public class Boleto : BaseTenantEntity
    {
        private Cliente _cliente;
        private Guid _clienteId;
        private string _cobrancaId;
        private string _formaPagamentoCobrancaId;
        private JurosValueObject _juros;
        private DescontoValueObject _desconto;
        private MultaValueObject _multa;
        private decimal _valor;
        private string _urlBoleto;

        public Guid ClienteId => _clienteId;
        public Cliente Cliente => _cliente;
        public string CobrancaId => _cobrancaId;
        public string FormaPagamentoCobrancaId => _formaPagamentoCobrancaId;
        public string UrlBoleto => _urlBoleto;
        public decimal Valor => _valor;
        public JurosValueObject Juros => _juros;
        public MultaValueObject Multa => _multa;
        public DescontoValueObject Desconto => _desconto;

        public Boleto(Guid clienteId, string cobrancaId, string formaPagamentoCobrancaId, decimal valor,
            JurosValueObject juros = null, MultaValueObject multa = null, DescontoValueObject desconto = null)
        {
            _clienteId = clienteId;
            _cobrancaId = cobrancaId;
            _formaPagamentoCobrancaId = formaPagamentoCobrancaId;
            _valor = valor;
            _juros = juros;
            _multa = multa;
            _desconto = desconto;

            AddEvent(new BoletoCriadoEvent(Id.ToString()));
        }
    }
}