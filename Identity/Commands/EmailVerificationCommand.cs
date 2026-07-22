using Identity.DTO;
using MediatR;

namespace Identity.Commands;

public record EmailVerificationCommand(
    Guid UserId,
    string code) : IRequest<UserResponse>;