using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using note_webapi.Interfaces;
using note_webapi.Models;

namespace note_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserRepository repo) : ControllerBase
{
    private readonly IUserRepository _repo = repo;
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        return user == null ? NotFound(new { message = "user not found!" }) : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        var result = await _repo.CreateAsync(user);
    
        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        user.Id = result.Data;
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        if (id != user.Id) return BadRequest();
    
        var result = await _repo.UpdateAsync(user);
    
        if (!result.Success)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        return result.Data ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _repo.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}