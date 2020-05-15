using System;

namespace Collectio.Domain.Base.Entities
{
    public class Cidade : BaseEntity
    {
        private string _nome;
        private string _codigo;
        private Estado _estado;
        private Guid _estadoId;

        public string Nome => _nome;
        public string Codigo => _codigo;

        public Estado Estado => _estado;
        public Guid EstadoId => _estadoId;

        public Cidade(string nome, string codigo, Guid estadoId)
        {
            _nome = nome;
            _codigo = codigo;
            _estadoId = estadoId;
        }
    }
}