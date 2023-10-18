using DbLogging;
using Microsoft.EntityFrameworkCore;
using Sample.Abstractions;
using Sample.Persistence.Entities;

namespace Sample.Persistence;

public class SampleRepository : ISampleRepository
{
    private readonly ApplicationDbContext _db;

    public DateTimeOffset Now { get; } = DateTimeOffset.UtcNow;

    public SampleRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ICompany>> GetCompaniesByIdsAsync(params Guid[] ids)
    {
        return await _db.Companies.AsNoTracking().Where(company => ids.Contains(company.Id)).Select(entity => entity.ToCore()).ToArrayAsync();
    }

    public async Task<IEnumerable<ICompany>> GetCompaniesByNamesAsync(params string[] names)
    {
        return await _db.Companies.AsNoTracking().Where(company => names.Contains(company.Name)).Select(entity => entity.ToCore()).ToArrayAsync();
    }

    public async Task<IEnumerable<IEmployee>> GetEmployeesByIdsAsync(params Guid[] id)
    {
        return await _db.Employees.AsNoTracking().Where(employee => id.Contains(employee.Id)).Select(entity => entity.ToCore()).ToArrayAsync();
    }

    public async Task<IEnumerable<IEmployee>> GetEmployeesByEmailsAsync(params string[] emails)
    {
        return await _db.Employees.AsNoTracking().Where(employee => emails.Contains(employee.Email)).Select(entity => entity.ToCore()).ToArrayAsync();
    }

    public async Task<IEnumerable<ICompanyEmployeeLink>> GetLinksByEmployeeIdsAsync(params Guid[] employeeIds)
    {
        return await _db.CompanyEmployeeLinks.AsNoTracking().Where(link => employeeIds.Contains(link.EmployeeId)).Select(entity => entity.ToCore()).ToArrayAsync();
    }

    public async Task<IEnumerable<ICompanyEmployeeLink>> GetLinksByCompanyIdsAsync(params Guid[] companyIds)
    {
        return await _db.CompanyEmployeeLinks.AsNoTracking().Where(link => companyIds.Contains(link.CompanyId)).Select(entity => entity.ToCore()).ToArrayAsync();
    }

    public ICompany CreateCompany(ICompanyInfo company)
    {
        var entity = company.ToEntity(Now);
        _db.Add(entity);
        return entity.ToCore();
    }

    public IEmployee CreateEmployee(IEmployeeInfo employee)
    {
        var entity = employee.ToEntity(Now);
        _db.Add(entity);
        return entity.ToCore();
    }

    public ICompanyEmployeeLink CreateCompanyEmployeeLinkAsync(Guid companyId, Guid employeeId, Title title)
    {
        var entity = new CompanyEmployeeLinkEntity
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            EmployeeId = employeeId,
            EmployeeTitle = title.ToEntity()
        };
        _db.Add(entity);
        return entity.ToCore();
    }

    public async Task<IEnumerable<ISystemLogEntry>> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync(Now);
    }
}