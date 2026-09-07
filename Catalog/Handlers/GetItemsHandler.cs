using System.Security.Claims;
using Catalog.DTO;
using Catalog.Repositories;
using MediatR;
using Profile.Commands;


namespace Profile.Handlers;

public class GetItemsHandler: IRequestHandler<GetItemsCommand,FoundItemsResponce>
{
    private readonly ICatalogRepository _catalogRepository;

    public GetItemsHandler(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }
    public async Task<FoundItemsResponce> Handle(
        GetItemsCommand request, CancellationToken cancellationToken)
    {
       return new FoundItemsResponce(await _catalogRepository.GetItemsAsync(request.UserId, request.page, request.pageSize));
    }
}