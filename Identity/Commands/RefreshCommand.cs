using Identity.DTO;
using MediatR;

namespace Identity.Commands;

public record RefreshCommand(string refreshToken): IRequest<UserResponse>;