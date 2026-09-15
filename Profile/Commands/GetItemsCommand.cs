using Catalog.DTO;
using MediatR;

namespace Profile.Commands;

public record GetItemsCommand(
    Guid UserId,
    int page,
    int pageSize):IRequest<FoundItemsResponce>;