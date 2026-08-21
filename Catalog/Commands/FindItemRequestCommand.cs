using Catalog.DTO;
using MediatR;

namespace Catalog.Commands;

public record FindItemRequestCommand(
    string request
    ): IRequest<FoundItemsResponce>;