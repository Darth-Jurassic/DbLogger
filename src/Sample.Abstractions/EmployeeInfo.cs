namespace Sample.Abstractions;

public class EmployeeInfo : IEmployeeInfo
{
    public string Email { get; }
    
    public Title Title { get; }

    public EmployeeInfo(string email, Title title)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));
        
        Email = email;
        Title = title;
    }
}