namespace Collectio.Domain.CobrancaAggregate
{
    public enum StatusCobranca
    {
        AguardandoCriacaoFormaPagamento,
        CriandoFormaPagamento,
        ErroCriarFormaPagamento,
        Pago,
        Vencido,
        Pendente
    }
}