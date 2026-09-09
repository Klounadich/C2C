using Catalog.DTO;
using MediatR;

namespace Catalog.Commands;

public record FindItemRequestCommand(
    string request,
    int page,
    int pageSize,
    Guid userId
    ): IRequest<FoundItemsResponce>;