namespace DbLogging.Entities;

public class SystemLogEntity
{
    public Guid Id { get; init; }
    public string ResourceType { get; init; }
    public Guid ResourceId { get; init; }
    public string Event { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string Changeset { get; init; }
    public string Comment { get; init; }
}