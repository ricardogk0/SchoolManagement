using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Dtos.Request;
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

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto studentCreateDto)
    {
        var userEmail = User.Identity?.Name ?? "ricardokevi@gmail.com";
        var createdStudent = await _studentService.CreateStudentAsync(studentCreateDto, userEmail);
        return Ok(createdStudent);
    }
}
