using Cart.DTO;
using MediatR;

namespace Cart.Commands;

public record GetCartCountRequest(
    Guid userId): IRequest<GetCartCountResponce>;