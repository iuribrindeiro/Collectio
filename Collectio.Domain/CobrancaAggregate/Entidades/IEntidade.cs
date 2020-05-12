using Collectio.Domain.Base.ValueObjects;

namespace Collectio.Domain.CobrancaAggregate.Entidades
{
    public interface IEntidade
    {
        string Nome { get; }
        CpfCnpjValueObject CpfCnpj { get; }
        Endereco Endereco { get; }
    }
}