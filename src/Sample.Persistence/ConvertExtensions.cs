using DbLogging;
using Sample.Abstractions;
using Sample.Persistence.Entities;

namespace Sample.Persistence;

public static class ConvertExtensions
{
    public static ICompany ToCore(this CompanyEntity entity)
    {
        return new Company(entity.Id, entity.Name, entity.CreatedAt);
    }
    
    public static IEmployee ToCore(this EmployeeEntity entity)
    {
        return new Employee(entity.Id, entity.Email, entity.Title.ToCore<Title>(), entity.CreatedAt);
    }

    public static ICompanyEmployeeLink ToCore(this CompanyEmployeeLinkEntity entity)
    {
        return new CompanyEmployeeLink(entity.CompanyId, entity.EmployeeId, entity.EmployeeTitle.ToCore<Title>());
    }

    public static CompanyEntity ToEntity(this ICompanyInfo company, DateTimeOffset now)
    {
        return new()
        {
            Id = new Guid(),
            Name = company.Name,
            CreatedAt = now
        };
    }
    
    public static EmployeeEntity ToEntity(this IEmployeeInfo employee, DateTimeOffset now)
    {
        return new()
        {
            Id = new Guid(),
            Email = employee.Email,
            Title = employee.Title.ToString(),
            CreatedAt = now
        };
    }

    public static string ToEntity<T>(this T value) where T : struct, Enum
    {
        return value.ToString().ToLowerInvariant();
    }

    public static T ToCore<T>(this string value) where T : struct, Enum
    {
        return Enum.Parse<T>(value, true);
    }
}