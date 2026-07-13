using Identity.DTO;
using MediatR;

namespace Identity.Commands;

public record AuthCommand(
    string Email,
    string Password) : IRequest<UserResponse>;