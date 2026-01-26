using kumablogB.Common;
using kumablogB.Models;
using kumablogB.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace kumablogB.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login ([FromBody] LoginRequest request)
    {
        (ServiceResult<LoginResult> result, string token) = await _service.Login(request.EmailOrUserId, request.Password);
        if (!result.Success)
        {
            return BadRequest(result);
        }

        CookieOptions cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,          // HTTPS のとき必須
            SameSite = SameSiteMode.Strict,
            Path = "/"
        };

        Response.Cookies.Append("auth_token", token, cookieOptions);


        return Ok(result);
    }

    [HttpGet("Session")]
    public async Task<IActionResult> CheckSession()
    {
        if (Request.Cookies.TryGetValue("auth_token", out string? authToken))
        {
            ServiceResult<string> result = await _service.CheckSession(authToken);

            if (!result.Success)
            {
                return Unauthorized(new { Error = result.Error });
            }
            else
            {
                return Ok(new { Message = "Valid session", UserId = result.Data });
            }
        }
        return Unauthorized(new { Error = "No valid session" });
    }
}
