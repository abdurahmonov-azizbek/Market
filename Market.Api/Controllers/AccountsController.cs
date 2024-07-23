using Market.Api.Extensions;
using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.Enums;
using Market.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(
    IUserService userService,
    IAccountService accountService) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public async ValueTask<IActionResult> GetCurrentUser()
    {
        var userId = long.Parse(HttpContext.GetValueByClaimType("Id"));

        var result = await userService.GetByIdAsync(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut("update/password")]
    [Authorize]
    public async ValueTask<IActionResult> UpdatePassword(UpdatePasswordDetails updatePasswordDetails)
    {
        var userId = long.Parse(HttpContext.GetValueByClaimType("Id"));

        var result = await accountService.UpdatePasswordAsync(userId, updatePasswordDetails);

        return result
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail", result));
    }

    [HttpPut("users/{userId:long}/grand/role/{role}")]
    [Authorize]
    public async ValueTask<IActionResult> GrandRole([FromRoute] long userId, [FromRoute] Role role)
    {
        var result = await accountService.GrandRoleAsync(new GrandRoleDetails
        {
            FromUserId = long.Parse(HttpContext.GetValueByClaimType("Id")),
            ToUserId = userId,
            Role = role
        });

        return result
           ? Ok(new Response(200, "Success", result))
           : BadRequest(new Response(400, "Fail", result));
    }
}