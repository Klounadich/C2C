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
                var token = await _jwtService.CreateTokenAsync(new JWTRequestCommand(user.Id.ToString() ,user.Username, user.Email , user.CreatedAt));
               
                if (!String.IsNullOrWhiteSpace(token))
                {
                    return new UserResponse(user.Username, token ,  user.CreatedAt);
                }
                else
                {
                    return new UserResponse(

                        "",
                        "failed",
                        DateTime.UtcNow);
                }
            }
        }
        catch (Exception ex)
        {
            // logg
        }

        return new UserResponse(

            "",
            "failed",
            DateTime.UtcNow);


    }
    }
