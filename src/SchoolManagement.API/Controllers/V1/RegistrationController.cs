using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Interfaces.Services;

namespace SchoolManagement.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/registrations")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterStudent([FromBody] RegistrationCreateDto registrationDto)
    {
        var result = await _registrationService.RegisterStudentAsync(registrationDto);
        return Ok(result);
    }

    [HttpGet("class/{classId}/students")]
    public async Task<IActionResult> GetStudentsByClass(Guid classId)
    {
        var result = await _registrationService.GetStudentsByClassAsync(classId);
        return Ok(result);
    }
}