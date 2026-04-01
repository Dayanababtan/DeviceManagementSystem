using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService) => _userService = userService;

    [HttpGet]
    public async Task<List<User>> Get() => await _userService.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var user = await _userService.GetAsync(id);
        if (user is null) return NotFound();
        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _userService.CreateAsync(newUser);
        return CreatedAtAction(nameof(Get), new { id = newUser.userId }, newUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User updatedUser)
    {
        var user = await _userService.GetAsync(id);
        if (user is null) return NotFound();
        updatedUser.userId = user.userId;
        await _userService.UpdateAsync(id, updatedUser);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userService.GetAsync(id);
        if (user is null) return NotFound();
        await _userService.RemoveAsync(id);
        return NoContent();
    }
}