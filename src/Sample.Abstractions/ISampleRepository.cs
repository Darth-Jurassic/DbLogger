namespace Sample.Abstractions;

public interface ISampleRepository
{
    Task<IEnumerable<ICompany>> GetCompaniesByIdsAsync(params Guid[] id);

    Task<IEnumerable<ICompany>> GetCompaniesByNamesAsync(params string[] names);

    Task<IEnumerable<IEmployee>> GetEmployeesByIdsAsync(params Guid[] id);

    Task<IEnumerable<IEmployee>> GetEmployeesByEmailsAsync(params string[] emails);

    ICompany CreateCompany(ICompanyInfo company);

    IEmployee CreateEmployee(IEmployeeInfo employee);

    ICompanyEmployeeLink CreateCompanyEmployeeLinkAsync(Guid companyId, Guid employeeId, Title title);

    Task<IEnumerable<ICompanyEmployeeLink>> GetLinksByEmployeeIdsAsync(params Guid[] employeeIds);

    Task<IEnumerable<ICompanyEmployeeLink>> GetLinksByCompanyIdsAsync(params Guid[] companyIds);

    Task<IEnumerable<ISystemLogEntry>> SaveChangesAsync();
}