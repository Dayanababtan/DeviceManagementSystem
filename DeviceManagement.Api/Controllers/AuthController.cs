using Microsoft.AspNetCore.Mvc;
using DeviceManagement.Api.Models;
using DeviceManagement.Api.Services;
using BCrypt.Net;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService) => _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(User newUser)
    {
        var existing = await _userService.GetByEmailAsync(newUser.email);
        if (existing != null) return BadRequest("Email already registered.");

        await _userService.CreateAsync(newUser);
        return Ok(new { message = "Registration successful" });
    }

   [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
    
        if (user == null) return Unauthorized("User not found");

        bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.password);
    
        if (!isValid) return Unauthorized("Wrong password");

    
        return Ok(new { userId = user.userId, name = user.name });
    }
public class LoginRequest {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
}