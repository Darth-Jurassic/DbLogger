namespace Sample.Api.Web.Controllers;

/// <summary>
/// Eployee information
/// </summary>
public class GetEmployeeResponse
{
    /// <summary>
    /// Employee ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Employee title
    /// </summary>
    public EmployeeTitle Title { get; init; }

    /// <summary>
    /// Employee email
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// Employee creation moment
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }
}