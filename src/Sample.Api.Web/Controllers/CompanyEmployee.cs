using Sample.Abstractions;
using Sample.Core;

namespace Sample.Api.Web.Controllers;

/// <summary>
/// Employee to add to company on creation
/// </summary>
public class CompanyEmployeeRequest
{
    /// <summary>
    /// Employee ID if an existing employee is to be added
    /// </summary>
    public Guid? Id { get; init; }
    
    /// <summary>
    /// Employee email if a new employee is to be created
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Employee title if a new employee is to be created
    /// </summary>
    public EmployeeTitle? Title { get; init; }
    
    internal IEmployeeInfo ToCore()
    {
        return new EmployeeInfo(Email.NotNull(), Title.NotNull().ToCore());
    }

}