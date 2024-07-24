using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.Entities;
using Market.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IPasswordHasherService passwordHasherService) : ControllerBase
{
    [HttpGet("hash")]
    public IActionResult Hash(string text) => Ok(passwordHasherService.Hash(text));

    [HttpPost("sign-up")]
    public async ValueTask<IActionResult> SignUp([FromBody] RegisterDetails registerDetails)
    {
        var result = await authService.RegisterAsync(registerDetails);

        return result
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail", result));
    }

    [HttpGet("sign-in")]
    public async ValueTask<IActionResult> SignIn([FromQuery] LoginDetails loginDetails)
    {
        var result = await authService.LoginAsync(loginDetails);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}