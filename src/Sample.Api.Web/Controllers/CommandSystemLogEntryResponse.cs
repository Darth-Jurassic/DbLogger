namespace Sample.Api.Web.Controllers;

/// <summary>
/// Command execution system log
/// </summary>
public class CommandSystemLogEntryResponse
{
    /// <summary>
    /// Related resource type
    /// </summary>
    public string ResourceType { get; init; }
    
    /// <summary>
    /// Related resource identifier
    /// </summary>
    public Guid ResourceId { get; init; }
    
    /// <summary>
    /// Related resource event
    /// </summary>
    public ResourceEvent Event { get; init; }
    
    /// <summary>
    /// Event moment
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }
    
    /// <summary>
    /// Event changeset
    /// </summary>
    public string Changeset { get; init; }
    
    /// <summary>
    /// Event description
    /// </summary>
    public string Comment { get; init; }
}