using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using MediatR;

namespace Catalog.Handlers;

public class GetItemHandler : IRequestHandler<GetItemCommand, FoundItemResponce>
{
    private readonly ICatalogRepository _repository;

    public GetItemHandler(ICatalogRepository repository)
    {
        _repository = repository;
    }
    public async Task<FoundItemResponce> Handle(GetItemCommand request, CancellationToken cancellationToken)
    {
        return await _repository.GetItemAsync(request.Id);
    }
}