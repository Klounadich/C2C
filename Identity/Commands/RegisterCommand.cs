using Identity.DTO;
using MediatR;

namespace Identity.Commands;

public record RegisterCommand(
    string Username,
    string Password,
    string Email) : IRequest<EmailVerificationResponce>;
    
    