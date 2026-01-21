using kumablogB.Models;
using kumablogB.Services;
using kumablogB.Common;
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
        ServiceResult<string> result = await _service.Login(request.UsernameOrEmail, request.Password);
        if (!result.Success || result.Data == null)
        {
            return new UnauthorizedObjectResult(new { Error = result.Error });
        }

        CookieOptions cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,          // HTTPS のとき必須
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append("auth_token", result.Data, cookieOptions);


        return Ok(new { message = "Logged in" });
    }
}
