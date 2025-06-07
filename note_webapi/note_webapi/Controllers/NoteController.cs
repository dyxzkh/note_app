using Microsoft.AspNetCore.Mvc;
using note_webapi.DTOs;
using note_webapi.Interfaces;
using note_webapi.Models;

namespace note_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NoteController(INoteRepository repo) : ControllerBase
{
    private readonly INoteRepository _repo = repo;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _repo.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _repo.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteDto dto)
    {
        var result = await _repo.CreateAsync(dto);
    
        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteDto dto)
    {
        var result = await _repo.UpdateAsync(id, dto);
    
        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        return result.Data ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repo.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}