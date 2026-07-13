using Identity.Commands;
using Identity.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("api/identity/[controller]")]

public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
   

    public UserController(IMediator mediator  )
    {
        _mediator = mediator;
        
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO userData)
    {
        var request = new RegisterCommand(userData.Username, userData.Password, userData.Email);
       var responce = await _mediator.Send(request);
       if (responce == null)
       {
           return BadRequest();
       }

       
       
       HttpContext.Response.Cookies.Append("auth_token", responce.Token, new CookieOptions
       {
           HttpOnly = true,
           SameSite = SameSiteMode.Lax,
           Secure = false,
           Expires = DateTime.Now.AddDays(1)
       });

        return Ok(new{message = "Successfully registered"});
    }
    [HttpPost("auth")]
    public async Task<IActionResult> AuthUser(AuthUserDTO userData)
    {
        var request = new AuthCommand(userData.Email, userData.Password);
        var responce = await _mediator.Send(request);
        if (responce.Token == "failed")
        {
            return BadRequest();
        }
        
        HttpContext.Response.Cookies.Append("auth_token", responce.Token, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = false,
            Expires = DateTime.Now.AddDays(1)
        });

        return Ok(new{message = "Successfully authorized"});
    }
    
}