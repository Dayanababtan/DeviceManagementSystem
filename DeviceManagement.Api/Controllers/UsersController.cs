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

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _userService.GetAsync(id);
        if (user is null) return NotFound();
        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _userService.CreateAsync(newUser);
        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }
}