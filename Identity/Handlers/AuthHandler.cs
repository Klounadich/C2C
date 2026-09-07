using System.Security.Cryptography;
using System.Text;
using Identity.Commands;
using Identity.DTO;
using Identity.Models;
using Identity.Repositories;
using Identity.Services;
using MediatR;

namespace Identity.Handlers;

public class AuthHandler : IRequestHandler<AuthCommand, UserResponse>
{
    private readonly IUserRepository _repository;
    private readonly IJWTService _jwtService;
    private readonly IMediator _mediator;

    public AuthHandler(IUserRepository repository, IJWTService jwtService, IMediator mediator)
    {
        _repository = repository;
        _jwtService = jwtService;
        _mediator = mediator;
    }

    public async Task<UserResponse> Handle(AuthCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _repository.GetUserByEmailAsync(request.Email);

            // ФИКС: раньше тут при user == null падало NRE на user.Password.
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return new UserResponse("", "failed", "", DateTime.UtcNow);
            }

            // ── Пароль верный. Если включена 2FA — токены НЕ выдаём,
            //    отправляем код и просим фронт подтвердить его отдельным эндпоинтом. ──
            if (user.TwoFactorEnabled)
            {
                var tfaResult = await _mediator.Send(new TFARequestCommand(user.Id.ToString()), cancellationToken);
                return new UserResponse(
                    user.Username, "", "", DateTime.UtcNow,
                    RequiresTwoFactor: true,
                    CodeId: tfaResult.CodeSent ? tfaResult.CodeId : null
                );
            }

            // ── Обычный логин без 2FA ──
            var token = await _jwtService.CreateTokenAsync(new JWTRequestCommand(user.Id.ToString(), user.Username,
                user.Email, user.CreatedAt , user.TwoFactorEnabled));
            var refresh_token = await _jwtService.CreateRefreshTokenAsync();
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refresh_token));
            var refresh_token_hashed = Convert.ToHexString(bytes);
            var expires = DateTime.UtcNow.AddDays(30);

            if (await _repository.UpdateRefreshTokenAsync(user.Id, refresh_token_hashed, Guid.NewGuid(), expires)
                && !string.IsNullOrWhiteSpace(token))
            {
                return new UserResponse(user.Username, token, refresh_token, user.CreatedAt);
            }
        }
        catch (Exception ex)
        {
            // logg
        }

        return new UserResponse("", "failed", "", DateTime.UtcNow);
    }
}
