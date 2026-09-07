using Catalog.Commands;
using Catalog.Repositories;
using MediatR;

namespace Catalog.Handlers;

public class RemoveItemHandler: IRequestHandler<RemoveItemCommand, bool>
{
    private readonly ICatalogRepository _catalogRepository;

    public RemoveItemHandler(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }
    public async Task<bool> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        return await _catalogRepository.RemoveItemAsync(request);
    }
}