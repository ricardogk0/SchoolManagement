using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.DTOs.Request;
using SchoolManagement.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolManagement.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/classes")]
public class ClassController : ControllerBase
{
    private readonly IClassService _classService;

    public ClassController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar turmas",
        Description = "Retorna a lista paginada de turmas aplicando filtros opcionais (ordenção, paginação e pesquisa).")]
    public async Task<IActionResult> GetAllClasses(Filters filters)
    {
        var classes = await _classService.GetAllAsync(filters);
        return Ok(classes);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Obter turma por ID",
        Description = "Retorna os detalhes da turma identificada pelo ID informado.")]
    public async Task<IActionResult> GetClassById(Guid id)
    {
        var classe = await _classService.GetByIdAsync(id);
        if (classe == null)
        {
            return NotFound();
        }
        return Ok(classe);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Criar turma",
        Description = "Cria uma nova turma com os dados fornecidos e retorna a turma criada.")]
    public async Task<IActionResult> CreateClass([FromBody] ClassCreateDto classCreateDto)
    {
        var userEmail = User.Identity?.Name ?? "ricardokevi@gmail.com";
        var created = await _classService.CreateClassAsync(classCreateDto, userEmail);
        return Ok(created);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Atualizar turma",
        Description = "Atualiza os dados da turma pelo ID. Retorna 404 caso não seja encontrada.")]
    public async Task<IActionResult> UpdateClass(Guid id, [FromBody] ClassUpdateDto classUpdateDto)
    {
        var userEmail = User.Identity?.Name ?? "ricardokevi@gmail.com";
        var updated = await _classService.UpdateClassAsync(id, classUpdateDto, userEmail);
        if (updated == null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [HttpPatch("{id}/soft-delete")]
    [SwaggerOperation(
        Summary = "Desativar turma (soft delete)",
        Description = "Marca a turma como inativa sem removê-la fisicamente do banco de dados.")]
    public async Task<IActionResult> SoftDeleteClass(Guid id)
    {
        var userEmail = User.Identity?.Name ?? "ricardokevi@gmail.com";
        var result = await _classService.SoftDeleteClassAsync(id, userEmail);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
