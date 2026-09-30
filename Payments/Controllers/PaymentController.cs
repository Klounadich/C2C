using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Commands.CreatePayment;
using Payments.Application.Commands.RefundPayment;
using Payments.Application.Queries;
using Payments.Controllers.Contracts;
using Payments.Domain;

namespace Payments.Controllers;
[ApiController]
[Route("api/payments/[controller]")]
public class PaymentController: ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook(
        PaymentWebhookRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePayment(CreatePaymentRequest request ) 
    {
        var user = HttpContext.User;
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _mediator.Send(new CreatePaymentCommand(request.OrderId, userId , request.idempotencyKey));
        return Ok(result);
        
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetPayment(Guid id) 
    {
        var query = new GetPaymentQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelPayment(Guid id) //CancelPaymentCommand
    {
        return Ok();
    }

    [HttpPost("{id}/refund")]
    [Authorize]
    public async Task<IActionResult> RefundPayment(Guid id) //RefundPaymentCommand
    {
        
        var user = HttpContext.User;
        var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var result = await _mediator.Send(new RefundPaymentCommand(id, userId));
        return Ok(result);
    }

}