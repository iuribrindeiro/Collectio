using Collectio.Domain.Base;
using System;

namespace Collectio.Domain.ClienteAggregate
{
    public class Cliente : BaseTenantEntity
    {
        //TO CREATE
        public Cliente(string nome)
        {
            Nome = nome;
        }

        //TO UPDATE
        public Cliente(Guid id, string nome) : base(id)
        {
            Nome = nome;
        }

        public string Nome { get; set; }
    }
}
