using MediatR;

namespace Cart.Commands;

public record AddItemToCartRequest(
    Guid CartId
    ) : IRequest<bool>;