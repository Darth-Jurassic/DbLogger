using DbLogging.Entities;
using Microsoft.EntityFrameworkCore;
using Sample.Abstractions;

namespace DbLogging;

public static class ConvertExtensions
{
    public static SystemLogEntity ToEntity(this ISystemLogEntry entry)
    {
        return new SystemLogEntity
        {
            Id = Guid.NewGuid(),
            ResourceType = entry.ResourceType,
            ResourceId = entry.ResourceId,
            Event = entry.Event.ToString(),
            CreatedAt = entry.CreatedAt,
            Changeset = entry.Changeset,
            Comment = entry.Comment
        };
    }

    public static ResourceEvent ToCore(this EntityState state)
    {
        return state switch
        {
            EntityState.Added => ResourceEvent.Created,
            EntityState.Modified => ResourceEvent.Updated,
            EntityState.Deleted => ResourceEvent.Deleted,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}