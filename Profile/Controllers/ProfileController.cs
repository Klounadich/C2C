using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Commands;

namespace Profile.Controllers;

[ApiController]
[Route("api/user/[controller]")]
public class ProfileController :ControllerBase
{
    private readonly IMediator _mediator;
    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpGet("get_my_items/{page}/{pageSize}")]
    public async Task<IActionResult> GetUserItems(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var user = HttpContext.User;
        if (user?.Identity == null || !user.Identity.IsAuthenticated)
            return Unauthorized();

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var command = new GetItemsCommand(userId, page, pageSize);
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}