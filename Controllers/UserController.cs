using kumablogB.Services;
using kumablogB.Models;
using Microsoft.AspNetCore.Mvc;

namespace kumablogB.Controllers;

[ApiController]
[Route("api/Users")]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Users>? users = await _service.GetAllUsersAsync();
        if (users == null || users.Count == 0) return NotFound("No users found.");
        return Ok(users);
    }
}