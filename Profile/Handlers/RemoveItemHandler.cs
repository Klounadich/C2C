using Catalog.Commands;
using MediatR;
using Profile.Repositories;

namespace Catalog.Handlers;

public class RemoveItemHandler: IRequestHandler<RemoveItemCommand, bool>
{
    private readonly IProfileRepository _catalogRepository;

    public RemoveItemHandler(IProfileRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }
    public async Task<bool> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        return await _catalogRepository.RemoveItemAsync(request);
    }
}