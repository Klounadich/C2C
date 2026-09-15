using MediatR;

namespace Catalog.Commands;

public record RemoveItemCommand(
    Guid ItemId, Guid userId): IRequest<bool>;