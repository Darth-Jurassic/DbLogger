namespace Sample.Abstractions;

public class Employee : EmployeeInfo, IEmployee
{
    public Employee(Guid id, string email, Title title, DateTimeOffset createdAt) :
        base(email, title)
    {
        Id = id;
        CreatedAt = createdAt;
    }

    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}