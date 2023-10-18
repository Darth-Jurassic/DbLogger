using System.ComponentModel.DataAnnotations;
using Sample.Abstractions;
using Sample.Core;

namespace Sample.Api.Web.Controllers;

/// <summary>
/// Employee creation request
/// </summary>
public class CreateEmployeeRequest
{
    /// <summary>
    /// Employee email
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string? Email { get; init; }

    /// <summary>
    /// Employee title
    /// </summary>
    [Required]
    public EmployeeTitle? Title { get; init; }

    /// <summary>
    /// Ids of companies to add employee to
    /// </summary>
    public Guid[]? CompanyIds { get; init; }

    internal IEmployeeInfo ToCore()
    {
        return new EmployeeInfo(Email.NotNull(), Title.NotNull().ToCore());
    }
}