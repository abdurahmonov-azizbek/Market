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
[Authorize(Roles = nameof(Role.SuperAdmin))]
public class ProductItemsController(
    IProductItemService productItemService,
    IProductService productService) : ControllerBase
{
    [HttpGet("get-by-code/{code:long}")]
    public async ValueTask<IActionResult> GetByCode([FromRoute] long code)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var productItem = (await productItemService.GetAll(userId))
            .FirstOrDefault(x => x.Code == code)
                ?? throw new EntityNotFoundException(typeof(ProductItem));

        var product = await productService.GetByIdAsync(productItem.Id)
            ?? throw new EntityNotFoundException(typeof(Product));

        var fullProduct = new FullProduct
        {
            Product = product,
            ProductItem = productItem
        };

        return Ok(new Response(200, "Success", fullProduct));
    }

    [HttpGet("get-by-key/{key}")]
    public async ValueTask<IActionResult> GetByCode([FromRoute] string key)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var productItems = (await productItemService.GetAll(userId))
            .Where(x => x.Title.Contains(key, StringComparison.OrdinalIgnoreCase))
                ?? throw new EntityNotFoundException(typeof(ProductItem));

        var result = new List<FullProduct>();

        foreach (var productItem in productItems)
        {
            var product = await productService.GetByIdAsync(productItem.Id)
                ?? throw new EntityNotFoundException(typeof(Product));

            var fullProduct = new FullProduct
            {
                Product = product,
                ProductItem = productItem
            };

            result.Add(fullProduct);
        }

        return Ok(new Response(200, "Success", result));
    }

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
