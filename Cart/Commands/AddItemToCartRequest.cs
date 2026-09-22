using MediatR;

namespace Cart.Commands;

public record AddItemToCartRequest(
 Guid ItemId 
    ) : IRequest<bool>;