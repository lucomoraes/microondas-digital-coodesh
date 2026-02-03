namespace Web.Microondas.Domain.DomainException;

public class DomainException(string message, Exception? inner = null) : Exception(message, inner);

