namespace Sample.Abstractions;

public class Company : CompanyInfo, ICompany
{
    public Company(Guid id, string name, DateTimeOffset createdAt) :
        base(name)
    {
        Id = id;
        CreatedAt = createdAt;
    }

    public Guid Id { get; }
    public DateTimeOffset CreatedAt { get; }
}