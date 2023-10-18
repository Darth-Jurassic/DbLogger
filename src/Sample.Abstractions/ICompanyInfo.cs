namespace Sample.Abstractions;

public interface ICompanyInfo
{
    string Name { get; }
}

public class CompanyInfo : ICompanyInfo
{
    public string Name { get; }

    public CompanyInfo(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

        Name = name;
    }
}