using Market.Api.Extensions;
using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.DTOs;
using Market.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin,System")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(UserDTO userDTO)
    {
        var userRole = HttpContext.GetValueByClaimType("Role");
        var role = (Role)Enum.Parse(typeof(Role), userRole);

        if (role > userDTO.Role)
            throw new InvalidOperationException("Access denied!");

        var result = await userService.CreateAsync(userDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var result = await userService.GetAllAsync();

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet("{userId}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid userId)
    {
        var result = await userService.GetByIdAsync(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(Guid userId, UserDTO userDTO)
    {
        var result = await userService.UpdateAsync(userId, userDTO);
        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{userId}")]
    public async ValueTask<IActionResult> DeleteById(Guid userId)
    {
        var result = await userService.DeleteById(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
