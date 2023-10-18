using DbLogging;
using Sample.Abstractions;

namespace Sample.Persistence.Entities;

public class CompanyEmployeeLinkEntity : ILoggingEntity
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; init; }
    public Guid EmployeeId { get; init; }
    public string EmployeeTitle { get; init; }
    
    public string GetChangeset(ResourceEvent resourceEvent)
    {
        return $"Id: {Id}, CompanyId: {CompanyId}, EmployeeId: {EmployeeId}, EmployeeTitle: {EmployeeTitle}";
    }

    public string GetComment(ResourceEvent resourceEvent)
    {
        return resourceEvent switch
        {
            ResourceEvent.Created => $"Employee {EmployeeId} added to company {CompanyId} as a {EmployeeTitle}",
            ResourceEvent.Updated => $"Employee {EmployeeId} updated in company {CompanyId} as a {EmployeeTitle}",
            ResourceEvent.Deleted => $"Employee {EmployeeId} removed in company {CompanyId}",
            _ => throw new ArgumentOutOfRangeException(nameof(resourceEvent), resourceEvent, null)
        };
    }

}