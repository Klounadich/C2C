using System.Security.Claims;
using Identity.Commands;
using Identity.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;

[ApiController]
[Route("api/identity/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // ───────────────────────────────────
    //  2FA: ВКЛЮЧЕНИЕ ИЗ НАСТРОЕК ПРОФИЛЯ (юзер уже авторизован)
    // ───────────────────────────────────

    [Authorize]
    [HttpPost("2fa/enable")]
    public async Task<IActionResult> Enable2fa()
    {
        var user = HttpContext.User;
        var responce = await _mediator.Send(new TFARequestCommand(user.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        if (responce.CodeSent)
        {
            return Ok(responce);
        }
        return BadRequest(responce);
    }

    // Раньше подтверждение 2FA-кода шло через /email-verification (не предназначен
    // для этого — там ставились новые auth-куки и EmailConfirmed, что не нужно здесь,
    // юзер уже залогинен). Теперь отдельный эндпоинт: только флиппает TwoFactorEnabled.
    [Authorize]
    [HttpPost("2fa/enable/confirm")]
    public async Task<IActionResult> ConfirmEnable2fa(TwoFactorEnableConfirmCommand command)
    {
        var enabled = await _mediator.Send(command);
        if (!enabled)
            return BadRequest(new { message = "Не удалось включить 2FA" });

        return Ok(new { twoFactorEnabled = true });
    }

    // ───────────────────────────────────
    //  2FA: ВТОРОЙ ШАГ ПРИ ЛОГИНЕ (юзер ещё НЕ авторизован)
    // ───────────────────────────────────

    [HttpPost("2fa/login-verify")]
    public async Task<IActionResult> VerifyLogin2FA(TwoFactorLoginVerifyCommand command)
    {
        var responce = await _mediator.Send(command);
        if (responce.AcessToken == "failed")
            return BadRequest(new { message = "Неверный или истёкший код" });

        SetAuthCookies(responce.AcessToken, responce.RefreshToken);
        return Ok(new { message = "Successfully authorized" });
    }

    // ───────────────────────────────────
    //  РЕГИСТРАЦИЯ / ПОДТВЕРЖДЕНИЕ EMAIL
    // ───────────────────────────────────

    [HttpPost("email-verification")]
    public async Task<IActionResult> EmailVerification(EmailVerificationCommand command)
    {
        var responce = await _mediator.Send(command);

        SetAuthCookies(responce.AcessToken, responce.RefreshToken);
        return Ok(new { message = "Successfully registration" });
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

        return Ok(responce);
    }

    // ───────────────────────────────────
    //  ЛОГИН
    // ───────────────────────────────────

    [HttpPost("auth")]
    public async Task<IActionResult> AuthUser(AuthUserDTO userData)
    {
        var request = new AuthCommand(userData.Email, userData.Password);
        var responce = await _mediator.Send(request);

        if (responce.RequiresTwoFactor)
        {
            // Куки НЕ ставим — юзер ещё не авторизован, ждём код на /2fa/login-verify
            return Ok(new { requiresTwoFactor = true, codeId = responce.CodeId });
        }

        if (responce.AcessToken == "failed")
            return BadRequest(new { message = "Неверный email или пароль" });

        SetAuthCookies(responce.AcessToken, responce.RefreshToken);
        return Ok(new { message = "Successfully authorized" });
    }

    // ───────────────────────────────────
    //  ПРОЧЕЕ (без изменений)
    // ───────────────────────────────────

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var rawRefreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(rawRefreshToken))
            return Unauthorized();

        var request = new RefreshCommand(rawRefreshToken);
        var response = await _mediator.Send(request);

        if (response.RefreshToken == "failed")
            return Unauthorized();

        SetAuthCookies(response.AcessToken, response.RefreshToken, isRefresh: true);
        return Ok(new { message = "Refreshed" });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var user = HttpContext.User;
        if (user?.Identity == null || !user.Identity.IsAuthenticated)
            return Unauthorized();

        return Ok(new
        {
            username = user.FindFirst(ClaimTypes.Name)?.Value,
            email = user.FindFirst(ClaimTypes.Email)?.Value,
            userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            registrationDate = user.FindFirst(ClaimTypes.UserData)?.Value
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Append("auth_token", "", new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = false,
            Expires = DateTime.Now.AddDays(-1)
        });

        return Ok(new { message = "Successfully logged out" });
    }

    // ───────────────────────────────────
    //  HELPER — раньше этот блок был продублирован в 4 местах
    // ───────────────────────────────────
    private void SetAuthCookies(string accessToken, string refreshToken, bool isRefresh = false)
    {
        Response.Cookies.Append("auth_token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = false,
            Expires = DateTime.Now.AddMinutes(10)
        });

        Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = false,
            Path = isRefresh ? "/api/identity/user/refresh" : "/",
            Expires = DateTime.Now.AddDays(70)
        });
    }
}
