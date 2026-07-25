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
    public AuthHandler(IUserRepository repository, IJWTService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }

    public async Task<UserResponse> Handle(AuthCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _repository.GetUserByEmailAsync(request.Email);
            
            if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                if (user.TwoFactorEnabled)
                {
                    
                    
                }
                var token = await _jwtService.CreateTokenAsync(new JWTRequestCommand(user.Id.ToString(), user.Username,
                    user.Email, user.CreatedAt));
                var refresh_token = await _jwtService.CreateRefreshTokenAsync();
                var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refresh_token));
                var refresh_token_hashed = Convert.ToHexString(bytes);
                var expires = DateTime.UtcNow.AddDays(30);
                if (await _repository.UpdateRefreshTokenAsync(user.Id, refresh_token_hashed, Guid.NewGuid(), expires)){
                    if (!String.IsNullOrWhiteSpace(token))
                    {
                        return new UserResponse(user.Username, token, refresh_token, user.CreatedAt);
                    }
                    else
                    {
                        return new UserResponse(

                            "",
                            "failed",
                            "",
                            DateTime.UtcNow);
                    }
            }
        }
    }
        catch (Exception ex)
        {
            // logg
        }

        return new UserResponse(

            "",
            "failed","",
            DateTime.UtcNow);


    }
    }
