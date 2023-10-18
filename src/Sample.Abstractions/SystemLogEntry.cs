namespace Sample.Abstractions;

public class SystemLogEntry : ISystemLogEntry
{
    public string ResourceType { get; init; }
    public Guid ResourceId { get; init; }
    public ResourceEvent Event { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Changeset { get; init; }
    public string Comment { get; init; }
}