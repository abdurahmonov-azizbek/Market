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
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(ProductDTO productDTO)
    {
        var userId = long.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await productService.CreateAsync(userId, productDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = long.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await productService.GetAll(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet("{productId:long}")]
    public async ValueTask<IActionResult> GetById([FromRoute] long productId)
    {
        var result = await productService.GetByIdAsync(productId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(long productId, ProductDTO productDTO)
    {
        var result = await productService.UpdateAsync(productId, productDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{productId:long}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] long productId)
    {
        var result = await productService.DeleteByIdAsync(productId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
