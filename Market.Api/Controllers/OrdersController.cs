using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.DTOs;
using Market.Domain.Entities;
using Market.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class OrdersController(
    IOrderService orderService,
    IUserService userService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(OrderDTO orderDTO)
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id")!);
        var result = await orderService.CreateAsync(userId, orderDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = Guid.Parse(HttpContext.GetValueByClaimType("Id")!);
        var currentUserRole = HttpContext.GetValueByClaimType("Role");

        if (currentUserRole == "Admin")
        {
            var result = await orderService.GetAll(userId);

            return result is not null
                ? Ok(new Response(200, "Success", result))
                : BadRequest(new Response(400, "Fail"));
        }
        else
        {
            var admins = (await userService.GetAllAsync(userId)).ToList();

            var result = new List<Order>();
            foreach (var admin in admins)
            {
                var adminOrders = await orderService.GetAll(admin.Id);

                if (adminOrders is not null)
                    result.AddRange(adminOrders);
            }

            result.AddRange(await orderService.GetAll(userId));

            return result is not null
                ? Ok(new Response(200, "Success", result))
                : BadRequest(new Response(400, "Fail"));
        }
    }

    [HttpGet("{orderId}")]
    public async ValueTask<IActionResult> GetById([FromRoute] Guid orderId)
    {
        var result = await orderService.GetByIdAsync(orderId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(Guid orderId, OrderDTO orderDTO)
    {
        var result = await orderService.UpdateAsync(orderId, orderDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{orderId}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] Guid orderId)
    {
        var result = await orderService.DeleteByIdAsync(orderId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}
