﻿using Market.Api.Models;
using Market.Application.Interfaces;
using Market.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(OrderDTO orderDTO)
    {
        var userId = long.Parse(HttpContext.GetServerVariable("Id")!);
        var result = await orderService.CreateAsync(userId, orderDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll()
    {
        var userId = long.Parse(HttpContext.GetServerVariable("Id")!);
        var result = await orderService.GetAll(userId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpGet("{orderId:long}")]
    public async ValueTask<IActionResult> GetById([FromRoute] long orderId)
    {
        var result = await orderService.GetByIdAsync(orderId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(long orderId, OrderDTO orderDTO)
    {
        var result = await orderService.UpdateAsync(orderId, orderDTO);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }

    [HttpDelete("{orderId:long}")]
    public async ValueTask<IActionResult> DeleteById([FromRoute] long orderId)
    {
        var result = await orderService.DeleteByIdAsync(orderId);

        return result is not null
            ? Ok(new Response(200, "Success", result))
            : BadRequest(new Response(400, "Fail"));
    }
}