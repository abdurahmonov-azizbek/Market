using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Application.Services;
using Market.Domain.DTOs;
using Market.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(Role.SuperAdmin))]
public class DebtsController(IDebtService debtService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(DebtDTO debtDTO)
    {
        var userId = Guid.Parse(HttpContext.GetServerVariable("Id")!);
        var result = await debtService.CreateAsync(userId, debtDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = Guid.Parse(HttpContext.GetServerVariable("Id")!);
        var result = await debtService.GetAll(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet("{debtId}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid debtId)
    {
        var result = await debtService.GetByIdAsync(debtId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(Guid debtId, DebtDTO debtDTO)
    {
        var result = await debtService.UpdateAsync(debtId, debtDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{debtId}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid debtId)
    {
        var result = await debtService.DeleteByIdAsync(debtId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
