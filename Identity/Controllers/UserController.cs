using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("[api/identity/[controller]")]

public class UserController : ControllerBase
{
    public async Task<IActionResult> RegisterUser()
    {
        return Ok();
    }
    
    public async Task<IActionResult> AuthUser()
    {
        return Ok();
    }
    
}