namespace Sample.Abstractions;

public interface ICompanyEmployeeLink
{
    public Guid CompanyId { get; }

    public Guid EmployeeId { get; }

    public Title Title { get; }
}

public class CompanyEmployeeLink : ICompanyEmployeeLink
{
    public CompanyEmployeeLink(Guid companyId, Guid employeeId, Title title)
    {
        CompanyId = companyId;
        EmployeeId = employeeId;
        Title = title;
    }

    public Guid CompanyId { get; }

    public Guid EmployeeId { get; }

    public Title Title { get; }
}