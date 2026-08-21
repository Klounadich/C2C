using Catalog.Commands;
using MediatR;
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
    
    

}