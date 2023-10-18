using DbLogging;
using Sample.Abstractions;

namespace Sample.Persistence.Entities;

public class EmployeeEntity : ILoggingEntity
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Email { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    
    public string GetChangeset(ResourceEvent trackedEntryState)
    {
        return $"Id: {Id}, Title: {Title}, Email: {Email}, CreatedAt: {CreatedAt}";
    }
    
    public string GetComment(ResourceEvent trackedEntryState)
    {
        return trackedEntryState switch
        {
            ResourceEvent.Created => $"Employee with email {Email} was created as {Title}",
            ResourceEvent.Updated => $"Employee with email {Email} was updated as {Title}",
            ResourceEvent.Deleted => $"Employee with email {Email} was updated as {Title}",
            _ => throw new ArgumentOutOfRangeException(nameof(trackedEntryState), trackedEntryState, null)
        };
    }
}