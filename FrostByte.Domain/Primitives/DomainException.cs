namespace FrostByte.Domain.Primitives;

/// <summary>
///     Base exception for domain rule violations and invariant breaches.
///     Keep it framework-agnostic; throw from Domain when an invariant is violated.
/// </summary>
[Serializable]
public class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception inner) : base(message, inner)
    {
    }
}
