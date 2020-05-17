namespace Collectio.Domain.ClienteAggregate.BoletoModels
{
    public class DescontoValueObject
    {
        private bool _fixo;
        private decimal _valor;

        public bool Fixo => _fixo;
        public decimal Valor => _valor;

        public DescontoValueObject(bool fixo, decimal valor)
        {
            _fixo = fixo;
            _valor = valor;
        }
    }
}