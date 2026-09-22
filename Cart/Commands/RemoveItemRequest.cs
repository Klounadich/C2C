using MediatR;
using Shared.Responce;

namespace Cart.Commands;

public record RemoveItemRequest(
    Guid ItemId,
    Guid cartId);