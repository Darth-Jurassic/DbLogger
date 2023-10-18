using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Core;

namespace Sample.Api.Web.Controllers;

/// <summary>
/// API for managing companies
/// </summary>
[ApiController]
[Route(Constants.RoutePrefix + "/" + Constants.CompaniesControllerName)]
public class CompaniesController : ControllerBase
{
    private readonly ILogger<CompaniesController> _log;
    private readonly ISampleManager _sampleManager;

    /// <summary>
    /// Get employee information
    /// </summary>
    /// <param name="id">Company ID</param>
    /// <returns>Resulting company information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetEmployeeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyResponse>> GetCompanyAsync(Guid id)
    {
        try
        {
            var company = await _sampleManager.GetCompanyAsync(id);
            if (company is null)
                return NotFound();

            return Ok(company.ToResponse());
        }
        catch (Exception ex)
        {
            var message = $"Unable to get company: {ex.Message}";
            _log.LogError(ex, message);
            return Problem(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Create a new company
    /// </summary>
    /// <param name="request">Company creation request</param>
    /// <returns>Resulting company information</returns>
    [HttpPost(Name = "CreateCompany")]
    [ProducesResponseType(typeof(List<ValidationResult>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<CommandResponse<CompanyResponse>>> CreateCompanyAsync(CreateCompanyRequest request)
    {
        try
        {
            var context = new ValidationContext(request);
            var errors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, context, errors))
                return BadRequest(errors);

            var employeesToLink = request.Employees?.Where(x => x.Id.HasValue).Select(x => x.Id.NotNull()).ToArray();
            var employeesToCreate = request.Employees?.Where(x => !x.Id.HasValue).Select(x => x.ToCore()).ToArray();

            var (company, systemLog) = await _sampleManager.CreateCompanyAsync(
                request.ToCore(),
                employeesToLink, employeesToCreate);

            var link = Url.Link(Constants.RouteName, new
            {
                controller = Constants.CompaniesControllerName,
                id = company.Id
            }) ?? throw new InvalidOperationException("Unable to generate created company link");
            return Created(link, company.ToCommandResponse(systemLog));
        }
        catch (Exception ex)
        {
            var message = $"Unable to create company: {ex.Message}";
            _log.LogError(ex, message);
            return Problem(message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Creates a new instance of <see cref="CompaniesController"/>
    /// </summary>
    public CompaniesController(ILogger<CompaniesController> log,
        ISampleManager sampleManager)
    {
        _log = log;
        _sampleManager = sampleManager;
    }
}