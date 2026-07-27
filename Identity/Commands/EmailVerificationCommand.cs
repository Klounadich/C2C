using Identity.DTO;
using MediatR;

namespace Identity.Commands;

public record EmailVerificationCommand(
    Guid CodeId,
    string code) : IRequest<UserResponse>;