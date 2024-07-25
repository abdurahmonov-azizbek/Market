using Market.Api.Extensions;
using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(Role.SuperAdmin))]
public class StatisticsController(IStatisticService statisticService) : ControllerBase
{
    [HttpGet]
    public async ValueTask<IActionResult> Get(int year, int month, int day)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id"));

        var dateTime = new DateTime(year, month, day);
        var result = await statisticService.Get(userId, dateTime);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
