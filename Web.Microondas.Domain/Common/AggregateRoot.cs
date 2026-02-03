namespace Web.Microondas.Domain.Common;

public abstract class AggregateRoot
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
