namespace Collectio.Domain.Base.Entities
{
    public class Estado : BaseEntity
    {
        private string _uf;
        private string _nome;

        public string Nome => _nome;
        public string UF => _uf;

        public Estado(string nome, string uf)
        {
            _nome = nome;
            _uf = uf;
        }
    }
}