using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Interfaces.Services;

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
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _studentService.GetAllAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto studentCreateDto)
    {
        var userEmail = User.Identity?.Name ?? "ricardokevi@gmail.com";
        var createdStudent = await _studentService.CreateStudentAsync(studentCreateDto, userEmail);
        return Ok(createdStudent);
    }

    [HttpPut("{id}")]
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
