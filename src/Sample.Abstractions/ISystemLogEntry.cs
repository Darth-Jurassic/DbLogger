namespace Sample.Abstractions;

public interface ISystemLogEntry
{
    string ResourceType { get; }
    Guid ResourceId { get; }
    ResourceEvent Event { get; }
    DateTimeOffset CreatedAt { get; }
    string Changeset { get; }
    string Comment { get; }
}