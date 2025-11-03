using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(
        Summary = "Registrar estudante em uma turma",
        Description = "Realiza o vínculo de um estudante a uma turma específica e retorna os dados da matrícula.")]
    public async Task<IActionResult> RegisterStudent([FromBody] RegistrationCreateDto registrationDto)
    {
        var result = await _registrationService.RegisterStudentAsync(registrationDto);
        return Ok(result);
    }

    [HttpGet("class/{classId}/students")]
    [SwaggerOperation(
        Summary = "Listar estudantes de uma turma",
        Description = "Retorna a lista de estudantes matriculados na turma informada pelo ID.")]
    public async Task<IActionResult> GetStudentsByClass(Guid classId)
    {
        var result = await _registrationService.GetStudentsByClassAsync(classId);
        return Ok(result);
    }
}