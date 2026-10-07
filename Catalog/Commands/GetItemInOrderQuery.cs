using Catalog.DTO;
using MediatR;

namespace Catalog.Commands;

public record GetItemInOrderQuery(
    Guid id,
    Guid userId) : IRequest<FoundItemsResponce>;