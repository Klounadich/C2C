using Catalog.DTO;
using MediatR;

namespace Catalog.Commands;

public record GetItemCommand(
    Guid Id): IRequest<FoundItemResponce>;