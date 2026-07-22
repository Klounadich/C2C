using System.Security.Cryptography;
using System.Text;
using Identity.Commands;
using Identity.DTO;
using Identity.Models;
using Identity.Repositories;
using Identity.Services;
using MediatR;

namespace Identity.Handlers;

public class VerificationHandler : IRequestHandler<EmailVerificationCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJWTService _jwtService;

    public VerificationHandler(IUserRepository userRepository, IJWTService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<UserResponse> Handle(EmailVerificationCommand request, CancellationToken cancellationToken)
    {
        var bytess = SHA256.HashData(Encoding.UTF8.GetBytes(request.code));
        var code_hashed = Convert.ToHexString(bytess);
        var verified = await _userRepository.VerificateEmailAsync(request.UserId, code_hashed);
        if (verified.EmailConfirmed)
        {
            var RegistrationDate = DateTime.UtcNow;
            var acess_token = await _jwtService.CreateTokenAsync(new JWTRequestCommand(verified.Id.ToString(),
                verified.Username, verified.Email, RegistrationDate));
            var refresh_token = await _jwtService.CreateRefreshTokenAsync();
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refresh_token));
            var refresh_token_hashed = Convert.ToHexString(bytes);
            var expires = DateTime.UtcNow.AddDays(30);
            if (await _userRepository.UpdateRefreshTokenAsync(verified.Id, refresh_token_hashed, Guid.NewGuid(),
                    expires))
            {
                if (!String.IsNullOrWhiteSpace(acess_token))
                {
                    return new UserResponse(
                        verified.Username,
                        acess_token,
                        refresh_token,
                        DateTime.UtcNow
                    );
                }


            }
        }

        return new UserResponse(
            verified.Username,
            "failed",
            "failed",
            DateTime.UtcNow
        );
    }
}