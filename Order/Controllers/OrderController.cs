using System.Runtime.CompilerServices;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands.CreateOrder;
using Order.Application.Queries;
using Order.Application.Queries.GetOrders;
using Order.Controllers.Contracts;

namespace Order.Controllers;
[ApiController]
[Route("api/order/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrderController(IMediator mediator)
    {
         _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var user = HttpContext.User;
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _mediator.Send(new CreateOrderCommand(request.ProductIds,request.amount,userId , request.longitude , request.lantitude));
        return Ok(result);
    }

    [HttpGet("{orderId}")]
    [Authorize]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        var user = HttpContext.User;
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _mediator.Send(new GetOrderQuery(orderId ,userId));
        return Ok(result);
    }
    
    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> GetOrder()
    {
        var user = HttpContext.User;
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _mediator.Send(new GetOrdersQuery( userId));
        return Ok(result);
    }
    
}