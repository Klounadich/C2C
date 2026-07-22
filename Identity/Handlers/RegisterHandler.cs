using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Identity.Commands;
using Identity.DTO;
using Identity.Models;
using Identity.Repositories;
using Identity.Services;
using Identity.Services.SMTP;
using Identity.Services.SMTP.Models;
using MailKit;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Identity.Handlers;

public class RegisterHandler : IRequestHandler<RegisterCommand , EmailVerificationResponce>
{
    private readonly IUserRepository _repository;
    private readonly ISMTPSerivce _mailService;
    private readonly IJWTService _jwtService;
    public RegisterHandler(IUserRepository repository , IJWTService jwtService ,  ISMTPSerivce mailService)
    {
        _repository = repository;
        _jwtService = jwtService;
        _mailService = mailService;
    }

    public async Task<EmailVerificationResponce> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        
        
        if (await _repository.EmailExistsAsync(request.Email))
            throw new ValidationException("User with this email already exists");
        if (await _repository.UserNameExistsAsync(request.Username))
        throw new ValidationException("User with this UserName already exists");
        
        string rand_confirm_number = new Random().Next(1000, 9999).ToString();
        var bytess = SHA256.HashData(Encoding.UTF8.GetBytes(rand_confirm_number));
        var code_hash = Convert.ToHexString(bytess);
        var mailData = new MailData
        {
            To = new List<string> { request.Email },
            From = "PodberuConfirm@yandex.ru",
            DisplayName = "TradeHub",
            ReplyTo = "PodberuConfirm@yandex.ru",
            ReplyToName = "Поддержка TradeHub",
            Subject = "Код подтверждения почты",
            Body = rand_confirm_number
        }; 
        
        
     
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
                
                var sent = await _mailService.SendAsync(mailData, CancellationToken.None);
                
                if (sent)
                {
                    await _repository.SaveVerificationCodeAsync(code_hash , User.Id);
                    return new EmailVerificationResponce
                    {
                        UserId = User.Id,
                        CodeSent = true
                    };
                }
            }
            else
                    {
                        return new EmailVerificationResponce{
                            UserId=  User.Id,
                            CodeSent = false
                        };

                            
                    }
                
            
        }
        catch (Exception ex)
        {
            // logg
        }

        return new EmailVerificationResponce
        {
            UserId = User.Id,
            CodeSent = false
        };;



    }
}