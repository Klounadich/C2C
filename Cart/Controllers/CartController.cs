using System.Security.Claims;
using Cart.Commands;
using Cart.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responce;

namespace Cart.Controllers;
[ApiController]
[Route("api/cart/[controller]")]
public class CartController : ControllerBase
{
     private readonly IMediator _mediator; 
     
     
     public CartController(IMediator mediator)
     {
          _mediator = mediator;
     }

     [Authorize]
     [HttpGet("get")]
     public async Task<IActionResult> GetCart()
     {
          var user = HttpContext.User;
          var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          var command = new GetCartRequest(Guid.Parse(userId));
          var result = await _mediator.Send(command);
          return Ok(result);
     }

     [Authorize]
     [HttpPost("add")]
     public async Task<IActionResult> AddItem(AddItemToCartRequest command)
     {
          var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          var request = new AddItemDTO
          {
               itemId = command.ItemId,
               userId = userId
          };

          var result = await _mediator.Send(request);

          if (result.IsSuccess)
               return NoContent();

          return result.ErrorType switch
          {
               ErrorType.ItemUnavailable => Conflict(result.ErrorMessage),
               ErrorType.AlreadyInCart => Conflict(result.ErrorMessage),
               ErrorType.ItemNotFound => NotFound(result.ErrorMessage),
               ErrorType.OwnItem => Conflict(result.ErrorMessage),
               ErrorType.DatabaseError => StatusCode(500, result.ErrorMessage),
               _ => StatusCode(500)
          };
     }
     
     
}