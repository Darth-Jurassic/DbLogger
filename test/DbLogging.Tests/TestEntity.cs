using Sample.Abstractions;

namespace DbLogging.Tests;

public class TestEntity : ILoggingEntity
{
    public Guid Id { get; init; }

    public string GetChangeset(ResourceEvent trackedEntryState)
    {
        return $"Id: {Id}";
    }

    public string GetComment(ResourceEvent trackedEntryState)
    {
        return $"Test entity {Id} was {trackedEntryState.ToString().ToLowerInvariant()}";
    }
}