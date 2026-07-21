using System.Security.Cryptography;
using System.Text;
using Identity.Commands;
using Identity.DTO;
using Identity.Repositories;
using Identity.Services;
using MediatR;

namespace Identity.Handlers;

public class TokenRefreshHandler : IRequestHandler<RefreshCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJWTService _jwtService;

    public TokenRefreshHandler(IUserRepository userRepository ,IJWTService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }
    
    public async Task<UserResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(request.refreshToken));
        var refresh_token_hashed = Convert.ToHexString(bytes);
        var stored = await _userRepository.GetRefreshTokenAsync(refresh_token_hashed);
        if (stored is null || stored.ExpiresAt < DateTime.UtcNow || stored.RevokedAt != null) {
            return new UserResponse(

                "",
                "failed",
                "failed",
                DateTime.UtcNow);
    }

        await _userRepository.RevokeTokenAsync(stored);
        var new_token = await _jwtService.CreateRefreshTokenAsync();
        var bytes1 = SHA256.HashData(Encoding.UTF8.GetBytes(new_token));
        var new_refresh_token_hashed = Convert.ToHexString(bytes1);
       await _userRepository.UpdateRefreshTokenAsync(stored.UserId , new_refresh_token_hashed, stored.FamilyId , DateTime.UtcNow.AddDays(70));
       var user = await _userRepository.GetUserByIdAsync(stored.UserId.ToString());
       var new_acess_token = await _jwtService.CreateTokenAsync(new JWTRequestCommand(stored.UserId.ToString(), user.Username, user.Email , user.CreatedAt));
       if (new_token != null && new_acess_token != null)
       {
           return new UserResponse(
               user.Username,
               new_acess_token,
               new_token,
               DateTime.UtcNow);
       }
       
       return new UserResponse(

           "",
           "failed",
           "failed",
           DateTime.UtcNow);
    }
}