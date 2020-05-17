namespace Collectio.Domain.ClienteAggregate.BoletoModels
{
    public class JurosValueObject
    {
        private bool _fixo;
        private decimal _valor;

        public bool Fixo => _fixo;
        public decimal Valor => _valor;

        public JurosValueObject(bool fixo, decimal valor)
        {
            _fixo = fixo;
            _valor = valor;
        }
    }
}