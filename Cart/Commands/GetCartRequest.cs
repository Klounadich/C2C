using Cart.DTO;
using MediatR;
using Shared.Responce;

namespace Cart.Commands;

public record GetCartRequest(
    Guid userId): IRequest<CartResponce>;