using DbLogging.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sample.Abstractions;

namespace DbLogging;

public class LoggingDbContext : DbContext
{
    public DbSet<SystemLogEntity> SystemLog { get; init; }

    public new async Task<IReadOnlyCollection<ISystemLogEntry>> SaveChangesAsync(DateTimeOffset now,
        CancellationToken cancellationToken = default)
    {
        var result = CreateSystemLogEntries(now);
        await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    private IReadOnlyCollection<ISystemLogEntry> CreateSystemLogEntries(DateTimeOffset now)
    {
        if (!ChangeTracker.HasChanges())
            return ArraySegment<ISystemLogEntry>.Empty;

        var result = new List<ISystemLogEntry>();

        foreach (var trackedEntry in ChangeTracker.Entries().ToArray())
        {
            if (trackedEntry.Entity is not ILoggingEntity trackedEntity)
                continue;

            switch (trackedEntry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                case EntityState.Deleted:
                    var systemLogEntry = CreatedEntitySystemLogEntry(trackedEntry, trackedEntity, now);
                    result.Add(systemLogEntry);
                    SystemLog.Add(systemLogEntry.ToEntity());
                    break;
            }
        }

        return result.AsReadOnly();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SystemLogEntity>(projectRecord =>
        {
            projectRecord.ToTable("SystemLog");
            projectRecord.HasKey(record => record.Id);
        });
    }
    
    public LoggingDbContext(DbContextOptions options)
        : base(options)
    {
    }

    private static ISystemLogEntry CreatedEntitySystemLogEntry(EntityEntry trackedEntry, ILoggingEntity trackedEntity,
        DateTimeOffset now)
    {
        var resourceEvent = trackedEntry.State.ToCore();
        
        return new SystemLogEntry
        {
            ResourceType = trackedEntity.GetType().Name,
            ResourceId = trackedEntity.Id,
            Event = resourceEvent,
            CreatedAt = now,
            Changeset = trackedEntity.GetChangeset(resourceEvent),
            Comment = trackedEntity.GetComment(resourceEvent)
        };
    }
}