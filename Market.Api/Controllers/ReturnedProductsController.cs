using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Market.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class ReturnedProductsController(
    IReturnedProductService returnedProductService,
    IUserService userService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(ReturnedProductDTO returnedProductDTO)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await returnedProductService.CreateAsync(userId, returnedProductDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));

        var role = HttpContext.GetValueByClaimType("Role");
        if (role == nameof(Role.Admin))
        {
            var result = await returnedProductService.GetAllAsync(userId);

            return result is not null
                ? Ok(new Response(200, "Success", result))
                : BadRequest(new Response(400, "Fail"));
        }
        else
        {
            var result = new List<ReturnedProduct>();

            var admins = (await userService.GetAllAsync(userId)).ToList();
            foreach (var admin in admins)
            {
                var entities = await returnedProductService.GetAllAsync(admin.Id);
                result.AddRange(entities);
            }

            return result is not null
                ? Ok(new Response(200, "Success", result))
                : BadRequest(new Response(400, "Fail"));
        }
    }

    [HttpGet("{id:guid}")]
    public async ValueTask<IActionResult> GetById(Guid id)
    {
        var result = await returnedProductService.GetByIdAsync(id);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(Guid id, ReturnedProductDTO returnedProductDTO)
    {
        var result = await returnedProductService.UpdateAsync(id, returnedProductDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{id:guid}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var result = await returnedProductService.DeleteByIdAsync(id);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
