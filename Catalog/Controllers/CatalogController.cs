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

    [HttpPost("add_item")]
    [Authorize]
    public async Task<IActionResult> AddItem(AddItemCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
    
    

}