using System.ComponentModel.DataAnnotations;
using Identity.Commands;
using Identity.DTO;
using Identity.Models;
using Identity.Repositories;
using Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Identity.Handlers;

public class RegisterHandler : IRequestHandler<RegisterCommand , UserResponse>
{
    private readonly IUserRepository _repository;
    private readonly IJWTService _jwtService;
    public RegisterHandler(IUserRepository repository , IJWTService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }

    public async Task<UserResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        
        // exists checks
        if (await _repository.EmailExistsAsync(request.Email))
            throw new ValidationException("User with this email already exists");
        if (await _repository.UserNameExistsAsync(request.Username))
        throw new ValidationException("User with this UserName already exists");
        
        //next step push up notification 
     
        var User = new User()
        {
            Email = request.Email,
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),

        };
        try
        {
            if (await _repository.RegisterAsync(User))
            {
                var RegistrationDate = DateTime.UtcNow;
                
                var token = await _jwtService.CreateTokenAsync(new JWTRequestCommand(User.Id.ToString() ,User.Username, User.Email , RegistrationDate));
               
                if (!String.IsNullOrWhiteSpace(token))
                {
                    return new UserResponse(User.Username , token ,  RegistrationDate);
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