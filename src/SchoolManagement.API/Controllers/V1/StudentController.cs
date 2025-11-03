using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolManagement.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/students")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar estudantes",
        Description = "Retorna a lista paginada de estudantes com suporte a filtros de pesquisa e ordenação.")]
    public async Task<IActionResult> GetAllStudents(Filters filters)
    {
        var students = await _studentService.GetAllAsync(filters);
        return Ok(students);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Obter estudante por ID",
        Description = "Retorna os detalhes do estudante identificado pelo ID informado.")]
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return Ok(student);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Criar estudante",
        Description = "Cria um novo estudante com os dados fornecidos e retorna o registro criado.")]
    public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto studentCreateDto)
    {
        var userEmail = User.Identity?.Name ?? "ricardokevi@gmail.com";
        var createdStudent = await _studentService.CreateStudentAsync(studentCreateDto, userEmail);
        return Ok(createdStudent);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Atualizar estudante",
        Description = "Atualiza os dados do estudante pelo ID. Retorna 404 caso não seja encontrado.")]
    public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] StudentUpdateDto studentUpdateDto)
    {
        var userEmail = User.Identity?.Name ?? "ricardokevi@gmail.com";
        var updated = await _studentService.UpdateStudentAsync(id, studentUpdateDto, userEmail);
        if (updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [HttpPatch("{id}/soft-delete")]
    [SwaggerOperation(
        Summary = "Desativar estudante (soft delete)",
        Description = "Marca o estudante como inativo sem removê-lo fisicamente do banco de dados.")]
    public async Task<IActionResult> SoftDeleteStudent(Guid id)
    {
        var userEmail = User.Identity?.Name ?? "ricardokevi@gmail.com";
        var result = await _studentService.SoftDeleteStudentAsync(id, userEmail);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
