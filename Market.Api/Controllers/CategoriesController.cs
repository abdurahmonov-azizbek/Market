﻿using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.DTOs;
using Market.Domain.Enums;
using Market.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(CategoryDTO categoryDTO)
    {
        var result = await categoryService.CreateAsync(Guid.Parse(HttpContext.GetValueByClaimType("Id")), categoryDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));

        var result = await categoryService.GetAllAsync(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet("{categoryId}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid categoryId)
    {
        var result = await categoryService.GetByIdAsync(categoryId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(Guid id, CategoryDTO categoryDTO)
    {
        var result = await categoryService.UpdateAsync(id, categoryDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{categoryId}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid categoryId)
    {
        var result = await categoryService.DeleteByIdAsync(categoryId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
