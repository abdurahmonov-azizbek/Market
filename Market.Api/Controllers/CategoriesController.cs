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
[Authorize(Roles = nameof(Role.Admin))]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(CategoryDTO categoryDTO)
    {
        var result = await categoryService.CreateAsync(long.Parse(HttpContext.GetValueByClaimType("Id")), categoryDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = long.Parse(HttpContext.GetValueByClaimType("Id"));

        var result = await categoryService.GetAllAsync(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet("{categoryId:long}")]
    public async ValueTask<IActionResult> GetById([FromRoute] long id)
    {
        var result = await categoryService.GetByIdAsync(id);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(CategoryDTO categoryDTO)
    {
        var result = await categoryService.UpdateAsync(
            long.Parse(HttpContext.GetValueByClaimType("Id")), categoryDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{categoryId:long}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] long categoryId)
    {
        var result = await categoryService.DeleteByIdAsync(categoryId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
