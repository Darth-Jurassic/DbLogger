using Sample.Abstractions;

namespace Sample.Core;

public class SampleManager : ISampleManager
{
    private readonly ISampleRepository _sampleRepository;

    public SampleManager(ISampleRepository sampleRepository)
    {
        _sampleRepository = sampleRepository;
    }

    public async Task<ICompany?> GetCompanyAsync(Guid id)
    {
        return (await _sampleRepository.GetCompaniesByIdsAsync(new[] { id })).FirstOrDefault();
    }

    public async Task<IEmployee?> GetEmployeeAsync(Guid id)
    {
        return (await _sampleRepository.GetEmployeesByIdsAsync(new[] { id })).FirstOrDefault();
    }

    public async Task<Tuple<ICompany, IReadOnlyCollection<ISystemLogEntry>>> CreateCompanyAsync(
        ICompanyInfo newCompanyInfo,
        IReadOnlyCollection<Guid>? employeesToLink,
        IReadOnlyCollection<IEmployeeInfo>? employeesToAdd)
    {
        return await RetryAsync(async () =>
        {
            var company = (await _sampleRepository.GetCompaniesByNamesAsync(newCompanyInfo.Name)).FirstOrDefault();
            var links = new List<ICompanyEmployeeLink>();
            if (company is null)
                company = _sampleRepository.CreateCompany(newCompanyInfo);
            else
                links.AddRange(await _sampleRepository.GetLinksByCompanyIdsAsync(company.Id));

            // Let's link everyone by ID first
            if (employeesToLink?.Any() ?? false)
            {
                var employees = (await _sampleRepository.GetEmployeesByIdsAsync(employeesToLink.ToArray())).ToArray();

                links.AddRange(employees.Select(employee => _sampleRepository.CreateCompanyEmployeeLinkAsync(company.Id, employee.Id, employee.Title)));
                AssertUniqueTitles(links);
            }

            // We can still find some employees by email
            if (employeesToAdd?.Any() ?? false)
            {
                var employees = (await _sampleRepository.GetEmployeesByEmailsAsync(employeesToAdd.Select(employee => employee.Email).ToArray())).ToArray();
                if (employees.Length > 0)
                {
                    employeesToAdd = employeesToAdd.Where(newEmployee => employees.All(employee => newEmployee.Email != employee.Email)).ToArray();

                    links.AddRange(employees.Select(employee => _sampleRepository.CreateCompanyEmployeeLinkAsync(company.Id, employee.Id, employee.Title)));
                    AssertUniqueTitles(links);
                }
            }

            // We can create employees we haven't found
            if (employeesToAdd?.Any() ?? false)
            {
                var employees = employeesToAdd.Select(newEmployee => _sampleRepository.CreateEmployee(newEmployee));
                links.AddRange(employees.Select(employee => _sampleRepository.CreateCompanyEmployeeLinkAsync(company.Id, employee.Id, employee.Title)));
                AssertUniqueTitles(links);
            }
            
            var logs = await _sampleRepository.SaveChangesAsync();
            return new Tuple<ICompany, IReadOnlyCollection<ISystemLogEntry>>(company, logs.ToArray());
        });
    }

    public async Task<Tuple<IEmployee, IReadOnlyCollection<ISystemLogEntry>>> CreateEmployeeAsync(
        IEmployeeInfo newEmployeeInfo,
        IReadOnlyCollection<Guid>? companiesToLink)
    {
        return await RetryAsync(async () =>
        {
            var employee = (await _sampleRepository.GetEmployeesByEmailsAsync(newEmployeeInfo.Email)).FirstOrDefault();
            var links = new List<ICompanyEmployeeLink>();
            if (employee is null)
                employee = _sampleRepository.CreateEmployee(newEmployeeInfo);
            else
            {
                links.AddRange(await _sampleRepository.GetLinksByEmployeeIdsAsync(employee.Id));
                if (companiesToLink?.Any() ?? false)
                    companiesToLink = companiesToLink.Where(companyId => links.All(link => link.CompanyId != companyId)).ToArray();
            }

            if (companiesToLink?.Any() ?? false)
            {
                var companyIds = (await _sampleRepository.GetCompaniesByIdsAsync(companiesToLink.ToArray())).Select(company => company.Id).ToArray();
                // Additionally add existing links for the companies we've found
                links.AddRange(await _sampleRepository.GetLinksByCompanyIdsAsync(companyIds));
                links.AddRange(companyIds.Select(companyId => _sampleRepository.CreateCompanyEmployeeLinkAsync(companyId, employee.Id, employee.Title)));
                AssertUniqueTitles(links);
            }

            var logs = await _sampleRepository.SaveChangesAsync();
            return new Tuple<IEmployee, IReadOnlyCollection<ISystemLogEntry>>(employee, logs.ToArray());
        });
    }

    private static void AssertUniqueTitles(List<ICompanyEmployeeLink> links)
    {
        var duplicate = links.GroupBy(link => (link.CompanyId, link.Title), link => link.EmployeeId).FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
            throw new ArgumentException($"Company {duplicate.Key.CompanyId} already has a {duplicate.Key.Title.ToString().ToLowerInvariant()}");
    }

    private static async Task<TResult> RetryAsync<TResult>(Func<Task<TResult>> func)
    {
        // TODO: Use Polly
        for (var tries = 3; tries > 0; tries--)
        {
            try
            {
                return await func();
            }
            catch (ConflictException)
            {
                if (tries <= 1)
                    throw;
                await Task.Delay(100);
            }
        }

        throw new SystemException("Number of tries exceeded");
    }
}