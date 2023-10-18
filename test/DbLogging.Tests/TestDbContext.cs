using Microsoft.EntityFrameworkCore;

namespace DbLogging.Tests;

public class TestDbContext : LoggingDbContext
{
    public DbSet<TestEntity> TestEntities { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TestEntity>(projectRecord =>
        {
            projectRecord.ToTable("TestEntity");
            projectRecord.HasKey(x => x.Id);
        });
    }
    
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
}