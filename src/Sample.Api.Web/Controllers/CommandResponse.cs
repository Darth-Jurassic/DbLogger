using Sample.Core;

namespace Sample.Api.Web.Controllers;

/// <summary>
/// Command response
/// </summary>
/// <typeparam name="TResult">Command result</typeparam>
public class CommandResponse<TResult> where TResult : class
{
    /// <summary>
    /// Command result
    /// </summary>
    public TResult? Result { get; init; } = default!;

    /// <summary>
    /// System log generated during command execution
    /// </summary>
    public IReadOnlyCollection<CommandSystemLogEntryResponse>? SystemLog { get; init; }
}