using System.Runtime.CompilerServices;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands.CreateOrder;
using Order.Controllers.Contracts;

namespace Order.Controllers;
[ApiController]
[Route("api/payments/[controller]")]
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
        var result = await _mediator.Send(new CreateOrderCommand(request.ProductIds,request.amount,userId));
        return Ok(result);
    }
    
}