namespace Sample.Api.Web.Controllers;

/// <summary>
/// Company information
/// </summary>
public class CompanyResponse
{
    /// <summary>
    /// Company ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Company name
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Company creation moment
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }
}