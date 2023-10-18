using Sample.Abstractions;

namespace Sample.Core;

public interface ISampleManager
{
    Task<ICompany?> GetCompanyAsync(Guid id);

    Task<IEmployee?> GetEmployeeAsync(Guid id);

    Task<Tuple<ICompany, IReadOnlyCollection<ISystemLogEntry>>> CreateCompanyAsync(ICompanyInfo newCompanyInfo, IReadOnlyCollection<Guid>? employeesToLink, IReadOnlyCollection<IEmployeeInfo>? employeesToAdd);

    Task<Tuple<IEmployee, IReadOnlyCollection<ISystemLogEntry>>> CreateEmployeeAsync(IEmployeeInfo newEmployeeInfo, IReadOnlyCollection<Guid>? companyIds);
}