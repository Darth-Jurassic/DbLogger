using Sample.Abstractions;
using Sample.Api.Web.Controllers;
using Sample.Core;
using ResourceEvent = Sample.Abstractions.ResourceEvent;

namespace Sample.Api.Web;

internal static class ConvertExtensions
{
    internal static CommandResponse<EmployeeResponse> ToCommandResponse(this IEmployee employee,
        IEnumerable<ISystemLogEntry> systemLog)
    {
        return new CommandResponse<EmployeeResponse>
        {
            Result = employee.ToResponse(),
            SystemLog = systemLog?.Select(x => x.ToResponse()).ToArray()
        };
    }

    internal static EmployeeResponse ToResponse(this IEmployee employee)
    {
        return new EmployeeResponse
        {
            Id = employee.Id,
            Title = employee.Title.ToResponse(),
            Email = employee.Email,
            CreatedAt = employee.CreatedAt
        };
    }

    internal static CommandResponse<CompanyResponse> ToCommandResponse(this ICompany company,
        IEnumerable<ISystemLogEntry> systemLog)
    {
        return new CommandResponse<CompanyResponse>
        {
            Result = company.ToResponse(),
            SystemLog = systemLog?.Select(x => x.ToResponse()).ToArray()
        };
    }

    internal static CompanyResponse ToResponse(this ICompany company)
    {
        return new CompanyResponse
        {
            Id = company.Id,
            Name = company.Name,
            CreatedAt = company.CreatedAt
        };
    }

    internal static CommandSystemLogEntryResponse ToResponse(this ISystemLogEntry systemLogEntry)
    {
        return new CommandSystemLogEntryResponse
        {
            ResourceType = systemLogEntry.ResourceType,
            ResourceId = systemLogEntry.ResourceId,
            Event = systemLogEntry.Event.ToResponse(),
            CreatedAt = systemLogEntry.CreatedAt,
            Changeset = systemLogEntry.Changeset,
            Comment = systemLogEntry.Comment,
        };
    }

    internal static Controllers.ResourceEvent ToResponse(this ResourceEvent title)
    {
        return title switch
        {
            ResourceEvent.Created => Controllers.ResourceEvent.Created,
            ResourceEvent.Updated => Controllers.ResourceEvent.Updated,
            _ => throw new ArgumentOutOfRangeException(nameof(title), title, null)
        };
    }

    internal static EmployeeTitle ToResponse(this Title title)
    {
        return title switch
        {
            Title.Developer => EmployeeTitle.Developer,
            Title.Manager => EmployeeTitle.Manager,
            Title.Tester => EmployeeTitle.Tester,
            _ => throw new ArgumentOutOfRangeException(nameof(title), title, null)
        };
    }

    internal static Title ToCore(this EmployeeTitle src)
    {
        return src switch
        {
            EmployeeTitle.Developer => Title.Developer,
            EmployeeTitle.Manager => Title.Manager,
            EmployeeTitle.Tester => Title.Tester,
            _ => throw new ArgumentOutOfRangeException(nameof(src), src, null)
        };
    }

    internal static string NotNull(this string? src)
    {
        if (string.IsNullOrWhiteSpace(src))
            throw new ArgumentNullException(nameof(src));
        return src;
    }


    internal static T NotNull<T>(this T? src) where T : struct
    {
        if (!src.HasValue)
            throw new ArgumentNullException(nameof(src));
        return src.Value;
    }

    internal static T NotNull<T>(this T? src) where T : class
    {
        if (src is null)
            throw new ArgumentNullException(nameof(src));
        return src;
    }
}