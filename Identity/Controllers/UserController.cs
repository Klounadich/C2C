using Identity.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("[api/identity/[controller]")]

public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO userData)
    {
       var responce = await _mediator.Send(userData);
       if (responce == null)
       {
           return BadRequest();
       }
        return Ok(responce);
    }
    
    public async Task<IActionResult> AuthUser()
    {
        return Ok();
    }
    
}