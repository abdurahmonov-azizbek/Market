using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Enums;
using Market.Domain.Exceptions;
using Market.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductItemsController(
    IProductItemService productItemService,
    IProductService productService,
    IUserService userService,
    IProductOrchestrationService productOrchestrationService) : ControllerBase
{
    [HttpGet("get-by-code/{code:long}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> GetByCode([FromRoute] long code)
    {
        var result = await productOrchestrationService.GetByCodeAsync(code);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet("get-by-key/{key}")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public async ValueTask<IActionResult> GetByCode([FromRoute] string key)
    {
        var result = await productOrchestrationService.GetByKeyAsync(key);

        return result is not null ? Ok(new Response(200, "Success", result)) : BadRequest();
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public async ValueTask<IActionResult> Create(ProductItemDTO productItemDTO)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await productItemService.CreateAsync(userId, productItemDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    [Authorize]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var role = HttpContext.GetValueByClaimType("Role");

        if (role == nameof(Role.SuperAdmin))
        {
            var result = await productItemService.GetAll(userId);

            return result is not null
                ? Ok(new Response(200, "Success", result))
                : BadRequest(new Response(400, "Fail"));
        }
        else if (role == nameof(Role.Admin))
        {
            var user = await userService.GetByIdAsync(userId)
                ?? throw new EntityNotFoundException(typeof(User));

            var result = await productItemService.GetAll(user.CreatedBy);

            return result is not null
              ? Ok(new Response(200, "Success", result))
              : BadRequest(new Response(400, "Fail"));
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet("{productItemId}")]
    [Authorize]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid productItemId)
    {
        var result = await productItemService.GetByIdAsync(productItemId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public async ValueTask<IActionResult> Update(Guid productItemId, ProductItemDTO productItemDTO)
    {
        var result = await productItemService.UpdateAsync(productItemId, productItemDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{productItemId}")]
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid productItemId)
    {
        var result = await productItemService.DeleteByIdAsync(productItemId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
