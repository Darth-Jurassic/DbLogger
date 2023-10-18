using Sample.Abstractions;

namespace DbLogging;

public interface ILoggingEntity
{
    public Guid Id { get; }
    string GetChangeset(ResourceEvent trackedEntryState);
    string GetComment(ResourceEvent trackedEntryState);
}