using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.Enums;
using Market.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController(IStatisticService statisticService) : ControllerBase
{
    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet("full-statistics-by-date")]
    public async ValueTask<IActionResult> GetByDate(int year, int month, int day)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await statisticService.GetFull(userId, new DateTime(year, month, day));

        return result is not null ? Ok(new Response(200, "Success", result)) : BadRequest(new Response(400, "Fail"));
    }

    [Authorize(Roles = nameof(Role.SuperAdmin))]
    [HttpGet("full-statistics")]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await statisticService.GetFull(userId);

        return result is not null ? Ok(new Response(200, "Success", result)) : BadRequest(new Response(400, "Fail"));
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("orders-statistics-by-date")]
    public async ValueTask<IActionResult> GetOrderStat(int year, int month, int day)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await statisticService.Get(userId, new DateTime(year, month, day));

        return result is not null ? Ok(new Response(200, "Success", result)) : BadRequest(new Response(400, "Fail"));
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("orders-statistics")]
    public async ValueTask<IActionResult> GetOrderStat()
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));
        var result = await statisticService.Get(userId);

        return result is not null ? Ok(new Response(200, "Success", result)) : BadRequest(new Response(400, "Fail"));
    }
}
