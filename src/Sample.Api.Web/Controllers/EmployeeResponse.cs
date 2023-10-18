namespace Sample.Api.Web.Controllers;

/// <summary>
/// Employee information
/// </summary>
public class EmployeeResponse
{
    /// <summary>
    /// Resulting employee identifier
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Resulting employee email
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// Resulting employee title
    /// </summary>
    public EmployeeTitle Title { get; init; }

    /// <summary>
    /// Resulting employee creation moment
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }
}