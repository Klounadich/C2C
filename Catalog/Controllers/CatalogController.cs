using System.Security.Claims;
using Catalog.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Catalog.Controllers;

[ApiController]
[Route("api/catalog/[controller]")]
public class CatalogController : ControllerBase
{
    private  readonly IMediator _mediator;
    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("find")]
    public async Task<IActionResult> FindItems(FindItemRequestCommand request)
    {
       var responce = await _mediator.Send(request);
       return Ok(responce);
    }
    
    
    [HttpPost("find/category")]
    public async Task<IActionResult> FindItemsCategory(FindItemCategoryRequestCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("item")]
    public async Task<IActionResult> GetItem([FromQuery] Guid id)
    {
        var request = new GetItemCommand(id);
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("add_item")]
    [Consumes("multipart/form-data")]
    [Authorize]
    [RequestSizeLimit(5 * 1024 * 1024)]
    public async Task<IActionResult> AddItem([FromForm] AddItemCommand command)
    {
        var user = HttpContext.User;
        if (user?.Identity == null || !user.Identity.IsAuthenticated)
            return Unauthorized();
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        if (userIdClaim != command.UserId.ToString())
            return Forbid();
   
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    
    

}