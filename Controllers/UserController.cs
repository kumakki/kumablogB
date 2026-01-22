using kumablogB.Services;
using kumablogB.Models;
using kumablogB.Common;
using Microsoft.AspNetCore.Mvc;

namespace kumablogB.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] string? id, [FromQuery] string? userid)
    {
        if (id != null)
        {
            ServiceResult<UserResult> user = await _service.GetUserByIdAsync(id);
            if (user.Data == null) return NotFound("User not found.");
            return Ok(user.Data);
        }
        else if (userid != null)
        {
            ServiceResult<UserResult> user = await _service.GetUserByUserIdAsync(userid);
            if (user.Data == null) return NotFound("User not found.");
            return Ok(user.Data);
        }

        ServiceResult<List<UserResult>> users = await _service.GetAllUsersAsync();
        if (users.Data == null || users.Data.Count == 0) return NotFound("No users found.");
        return Ok(users.Data);
    }

    //[HttpGet]
    //public async Task<IActionResult> GetUserById([FromQuery] string? id, [FromQuery] string? userid)
    //{
    //    if (id != null)
    //    {
    //        ServiceResult<UserResult> user = await _service.GetUserByIdAsync(id);
    //        if (user.Data == null) return NotFound("User not found.");
    //        return Ok(user.Data);
    //    }
    //    else if (userid != null)
    //    {
    //        ServiceResult<UserResult> user = await _service.GetUserByUserIdAsync(userid);
    //        if (user.Data == null) return NotFound("User not found.");
    //        return Ok(user.Data);
    //    }
    //    return BadRequest("id or name is required");
    //}

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest cur)
    {
        ServiceResult<string> result = await _service.CreateUserAsync(cur);
        if (!result.Success) return BadRequest(result.Error);
        return Ok(result.Data);
    }


}