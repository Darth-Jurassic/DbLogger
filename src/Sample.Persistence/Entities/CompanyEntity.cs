using DbLogging;
using Sample.Abstractions;

namespace Sample.Persistence.Entities;

public class CompanyEntity : ILoggingEntity
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public string GetChangeset(ResourceEvent trackedEntryState)
    {
        return $"Id: {Id}, Name: {Name}, CreatedAt: {CreatedAt}";
    }

    public string GetComment(ResourceEvent trackedEntryState)
    {
        return trackedEntryState switch
        {
            ResourceEvent.Created => $"Company {Name} created",
            ResourceEvent.Updated => $"Company {Name} updated",
            ResourceEvent.Deleted => $"Company {Name} deleted",
            _ => throw new ArgumentOutOfRangeException(nameof(trackedEntryState), trackedEntryState, null)
        };
    }
}