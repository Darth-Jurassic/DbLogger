using System.ComponentModel.DataAnnotations;
using Sample.Abstractions;
using Sample.Core;

namespace Sample.Api.Web.Controllers;

/// <summary>
/// Company creation request
/// </summary>
public class CreateCompanyRequest
{
    /// <summary>
    /// Employee email
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; init; }
    
    /// <summary>
    /// Employees to add to company on creation
    /// </summary>
    public ICollection<CompanyEmployeeRequest>? Employees { get; init; }
    
    public ICompanyInfo ToCore()
    {
        return new CompanyInfo(Name.NotNull());
    }
}