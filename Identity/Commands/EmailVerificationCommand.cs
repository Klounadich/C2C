using Identity.DTO;
using MediatR;

namespace Identity.Commands;

public record EmailVerificationCommand(
    string Email,
    string code) : IRequest<UserResponse>;