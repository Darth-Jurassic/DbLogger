namespace Sample.Abstractions;

public interface IEmployee : IEmployeeInfo
{
    Guid Id { get; }
    
    DateTimeOffset CreatedAt { get; }
}

public interface ICompany : ICompanyInfo
{
    Guid Id { get; }
    
    DateTimeOffset CreatedAt { get; }
}
