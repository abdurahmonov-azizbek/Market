using Market.Api.Extensions;
using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ProductItemsController(IProductItemService productItemService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(ProductItemDTO productItemDTO)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await productItemService.CreateAsync(userId, productItemDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await productItemService.GetAll(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet("{productItemId}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid productItemId)
    {
        var result = await productItemService.GetByIdAsync(productItemId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(Guid productItemId, ProductItemDTO productItemDTO)
    {
        var result = await productItemService.UpdateAsync(productItemId, productItemDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{productItemId}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid productItemId)
    {
        var result = await productItemService.DeleteByIdAsync(productItemId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
