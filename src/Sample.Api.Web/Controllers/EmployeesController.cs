using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Core;

namespace Sample.Api.Web.Controllers;

/// <summary>
/// API for managing employees
/// </summary>
[ApiController]
[Route(Constants.RoutePrefix + "/" + Constants.EmployeesControllerName)]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _log;
    private readonly ISampleManager _sampleManager;

    /// <summary>
    /// Get employee information
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <returns>Resulting employee information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetEmployeeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetEmployeeResponse>> GetEmployeeAsync(Guid id)
    {
        try
        {
            var employee = await _sampleManager.GetEmployeeAsync(id);
            if (employee is null)
                return NotFound();

            return Ok(employee.ToResponse());
        }
        catch (Exception ex)
        {
            var message = $"Unable to get employee: {ex.Message}";
            _log.LogError(ex, message);
            return Problem(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    /// <param name="request">Employee creation request</param>
    /// <returns>Resulting employee information</returns>
    [HttpPost]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync(CreateEmployeeRequest request)
    {
        try
        {
            var context = new ValidationContext(request);
            var errors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, context, errors))
                return BadRequest(errors);

            var (employee, systemLog) = await _sampleManager.CreateEmployeeAsync(
                request.ToCore(),
                request.CompanyIds);

            var link = Url.Link(Constants.RouteName, new
            {
                controller = Constants.EmployeesControllerName,
                id = employee.Id
            }) ?? throw new InvalidOperationException("Unable to generate created employee link");
            return Created(link, employee.ToCommandResponse(systemLog));
        }
        catch (Exception ex)
        {
            var message = $"Unable to create employee: {ex.Message}";
            _log.LogError(ex, message);
            return Problem(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Creates a new instance of <see cref="EmployeesController"/>
    /// </summary>
    public EmployeesController(ILogger<EmployeesController> log,
        ISampleManager sampleManager)
    {
        _log = log;
        _sampleManager = sampleManager;
    }
}