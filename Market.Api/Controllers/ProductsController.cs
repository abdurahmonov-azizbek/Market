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
public class ProductsController(
    IProductService productService,
    IUserService userService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public async ValueTask<IActionResult> Create(ProductDTO productDTO)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await productService.CreateAsync(userId, productDTO);

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
            var result = await productService.GetAll(userId);

            return result is not null
                ? Ok(new Response(200, "Success", result))
                : BadRequest(new Response(400, "Fail"));

        }
        else if (role == nameof(Role.Admin))
        {
            var user = await userService.GetByIdAsync(userId);

            var result = await productService.GetAll(user.CreatedBy);

            return result is not null
                ? Ok(new Response(200, "Success", result))
                : BadRequest(new Response(400, "Fail"));
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet("{productId}")]
    [Authorize]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid productId)
    {
        var result = await productService.GetByIdAsync(productId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public async ValueTask<IActionResult> Update(Guid productId, ProductDTO productDTO)
    {
        var result = await productService.UpdateAsync(productId, productDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{productId}")]
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid productId)
    {
        var result = await productService.DeleteByIdAsync(productId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
