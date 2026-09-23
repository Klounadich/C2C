using System.Security.Claims;
using Favorites.Commands;
using Favorites.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responce;


namespace Favorites.Controllers;
[ApiController]
[Route("api/favorites/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly IMediator _mediator; 
    public FavoritesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddItem(AddItemToFavoritesRequest command)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var request = new AddItemDTO
        {
            itemId = command.itemId,
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

    [Authorize]
    [HttpGet("items")]
    public async Task<IActionResult> GetItems([FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var request = new GetFavoritesItemsDTO()
        {
            page = page,
            pageSize = pageSize,
            UserId = userId
            
        };
        var result = await _mediator.Send(request);
        return Ok(result);
    }
    
}