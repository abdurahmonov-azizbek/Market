using Market.Api.Extensions;
using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Market.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "System,SuperAdmin")]
public class UsersController(
    IUserService userService,
    IPasswordHasherService passwordHasherService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(UserDTO userDTO)
    {
        var creatorUserRole = HttpContext.GetValueByClaimType("Role");
        var creatorUserId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));

        if (creatorUserRole == "Admin")
            throw new InvalidOperationException("You can't creat user!");

        if (creatorUserRole == "System" && userDTO.Role != Role.SuperAdmin)
            throw new InvalidOperationException("You can create only Super admins!");

        if (creatorUserRole == "SuperAdmin" && userDTO.Role != Role.Admin)
            throw new InvalidOperationException("You can create only admins!");

        userDTO.Password = passwordHasherService.Hash(userDTO.Password);
        var result = await userService.CreateAsync(creatorUserId, userDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await userService.GetAllAsync(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet("{userId}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid userId)
    {
        var id = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await userService.GetByIdAsync(userId);

        if (result is not null && result.CreatedBy != id)
            return BadRequest(new Response(400, "Fail"));

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(Guid userId, UserDTO userDTO)
    {
        var currentUserId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));

        var targetUser = await userService.GetByIdAsync(userId)
            ?? throw new EntityNotFoundException(typeof(User));

        if (targetUser.CreatedBy != currentUserId)
            throw new InvalidOperationException("You can't update this user!");

        var result = await userService.UpdateAsync(userId, userDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{userId}")]
    public async ValueTask<IActionResult> DeleteById(Guid userId)
    {
        var currentUserId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));

        var targetUser = await userService.GetByIdAsync(userId)
          ?? throw new EntityNotFoundException(typeof(User));

        if (targetUser.CreatedBy != currentUserId)
            throw new InvalidOperationException("You can't delete this user!");

        var result = await userService.DeleteById(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
