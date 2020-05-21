using System;
using Collectio.Domain.Base;
using Collectio.Domain.ClienteAggregate;
using Collectio.Domain.ClienteAggregate.Events;

namespace Collectio.Domain.BoletoAggregate
{
    public class Boleto : BaseOwnerEntity
    {
        private Cliente _cliente;
        private Guid _clienteId;
        private JurosValueObject _juros;
        private DescontoValueObject _desconto;
        private MultaValueObject _multa;
        private decimal _valor;
        private string _urlBoleto;
        private DateTime _vencimento;

        public Guid ClienteId => _clienteId;
        public Cliente Cliente => _cliente;
        public string UrlBoleto => _urlBoleto;
        public decimal Valor => _valor;
        public JurosValueObject Juros => _juros;
        public MultaValueObject Multa => _multa;
        public DescontoValueObject Desconto => _desconto;
        public DateTime Vencimento => _vencimento;

        public Boleto(Guid clienteId, DateTime vencimento, decimal valor, 
            JurosValueObject juros = null, MultaValueObject multa = null, DescontoValueObject desconto = null)
        {
            _clienteId = clienteId;
            _vencimento = vencimento;
            _valor = valor;
            _juros = juros;
            _multa = multa;
            _desconto = desconto;

            AddEvent(new BoletoCriadoEvent(Id.ToString()));
        }
    }
}