using DbLogging;
using Microsoft.EntityFrameworkCore;
using Sample.Persistence.Entities;

namespace Sample.Persistence;

public class ApplicationDbContext : LoggingDbContext
{
    public DbSet<CompanyEntity> Companies { get; init; }
    
    public DbSet<EmployeeEntity> Employees { get; init; }

    public DbSet<CompanyEmployeeLinkEntity> CompanyEmployeeLinks { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompanyEntity>(projectRecord =>
        {
            projectRecord.ToTable("Company");
            projectRecord.HasKey(x => x.Id);
            projectRecord.HasIndex(x => new { x.Name }).IsUnique();
        });

        modelBuilder.Entity<EmployeeEntity>(projectRecord =>
        {
            projectRecord.ToTable("Employee");
            projectRecord.HasKey(x => x.Id);
            projectRecord.HasIndex(x => new { x.Email }).IsUnique();
        });

        modelBuilder.Entity<CompanyEmployeeLinkEntity>(projectRecord =>
        {
            projectRecord.ToTable("Company_Employee");
            projectRecord.HasKey(x => x.Id);
            projectRecord.HasIndex(x => new { x.CompanyId, x.EmployeeTitle }).IsUnique();
        });
    }
    
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
}