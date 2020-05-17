using System;

namespace Collectio.Domain.ClienteAggregate.BoletoModels
{
    public class MultaValueObject
    {
        private bool _fixo;
        private decimal _valor;

        public bool Fixo => _fixo;
        public decimal Valor => _valor;

        public MultaValueObject(bool fixo, decimal valor)
        {
            _fixo = fixo;
            _valor = valor;
        }
    }
}