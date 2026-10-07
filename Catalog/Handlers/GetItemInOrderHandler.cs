using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using MediatR;

namespace Catalog.Handlers;

public class GetItemInOrderHandler : IRequestHandler<GetItemInOrderQuery, FoundItemsResponce>
{
    private readonly ICatalogRepository _repository;

    public GetItemInOrderHandler(ICatalogRepository repository)
    {
        _repository = repository;
    }
    public async Task<FoundItemsResponce> Handle(GetItemInOrderQuery request, CancellationToken cancellationToken)
    {
      return await  _repository.GetItemAsync(request.id, request.userId);
    }
}