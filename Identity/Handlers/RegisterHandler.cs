using System.ComponentModel.DataAnnotations;
using Identity.Commands;
using Identity.DTO;
using Identity.Models;
using Identity.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Identity.Handlers;

public class RegisterHandler : IRequestHandler<RegisterCommand , UserResponse>
{
    private readonly IUserRepository _repository;
    
    public RegisterHandler(IUserRepository repository)
    {
        _repository = repository;
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
            return await _repository.RegisterAsync(User);
        }
        catch (Exception ex)
        {
            // logg
        }

        return new UserResponse{};

    }
}