using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Identity.Commands;
using Identity.DTO;
using Identity.Repositories;
using Identity.Services;
using MediatR;

namespace Identity.Handlers;

// В отличие от VerificationHandler (подтверждение email при регистрации),
// этот хендлер НЕ трогает EmailConfirmed — это другой сценарий
// (юзер уже подтверждён, просто проходит второй фактор при входе).
public class TwoFactorLoginVerifyHandler : IRequestHandler<TwoFactorLoginVerifyCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJWTService _jwtService;

    public TwoFactorLoginVerifyHandler(IUserRepository userRepository, IJWTService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<UserResponse> Handle(TwoFactorLoginVerifyCommand request, CancellationToken cancellationToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(request.Code));
        var codeHashed = Convert.ToHexString(bytes);

        var userIdStr = await _userRepository.VerificateEmailAsync(request.CodeId, codeHashed);
        var user = await _userRepository.GetUserByIdAsync(userIdStr);
        if (user is null)
            throw new ValidationException("Пользователь не найден.");

        var accessToken = await _jwtService.CreateTokenAsync(
            new JWTRequestCommand(user.Id.ToString(), user.Username, user.Email, user.CreatedAt , user.TwoFactorEnabled));
        var refreshToken = await _jwtService.CreateRefreshTokenAsync();
        var refreshHashed = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
        var expires = DateTime.UtcNow.AddDays(30);

        if (await _userRepository.UpdateRefreshTokenAsync(user.Id, refreshHashed, Guid.NewGuid(), expires)
            && !string.IsNullOrWhiteSpace(accessToken))
        {
            return new UserResponse(user.Username, accessToken, refreshToken, user.CreatedAt);
        }

        return new UserResponse(user.Username, "failed", "", DateTime.UtcNow);
    }
}
