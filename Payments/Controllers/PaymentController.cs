using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreatePayment() // CreatePaymentCommand
    {
        return Ok();
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetPayment(Guid id) //GetPaymentQuery
    {
        return Ok();
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
        return Ok();
    }

}