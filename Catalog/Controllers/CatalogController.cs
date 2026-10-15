using System.Security.Claims;
using Catalog.Commands;
using Catalog.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responce;

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
    public async Task<IActionResult> FindItems(FindItemRequest request)
    {
        Guid? userId = Guid.TryParse(
            HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

        var command = new FindItemRequestCommand(request.request, request.page, request.pageSize, userId);
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("find/category")]
    public async Task<IActionResult> FindItemsCategory(FindItemCategoryRequest request)
    {
        Guid? userId = Guid.TryParse(
            HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

        var command = new FindItemCategoryRequestCommand(request.request, request.page, request.pageSize, userId);
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("item")]
    public async Task<IActionResult> GetItem([FromQuery] Guid id)
    {
        var request = new GetItemCommand(id);
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("item_in_order")]
    [Authorize]
    public async Task<IActionResult> GetItemInOrder([FromQuery] Guid id)
    {
        var user = HttpContext.User;
        var userId = Guid.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var request = new GetItemInOrderQuery(id, userId);
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
        if (!response)
            return BadRequest("Не удалось добавить товар в корзину");

        return Ok();
    }

    [HttpPost("report_item")]
    
    public async Task<IActionResult> ReportItem(ReportItemRequest request)
    {
        var user = HttpContext.User;
        var userIdClaim = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var command = new ReportItemDTO
        {
            text = request.text,
            userId = userIdClaim
        };
        var response = await _mediator.Send(command);
        if (response.IsSuccess)
            return NoContent();

        return response.ErrorType switch
        {
            ErrorType.ItemNotFound =>
                NotFound(response.ErrorMessage),

            ErrorType.Unauthorized =>
                Forbid(),

            ErrorType.DatabaseError =>
                StatusCode(500, response.ErrorMessage),

            _ => BadRequest(response.ErrorMessage)
        };
    }
    
    

}