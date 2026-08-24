using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using MediatR;

namespace Catalog.Handlers;

public class FindItemCategoryHandler : IRequestHandler<FindItemCategoryRequestCommand, FoundItemsResponce>
{
    private const string SynonymsLink =
        "https://ruwordnet.ru/ru/search/";
    private readonly SynonymousWordsParser _parser;
    private readonly ICatalogRepository _catalogRepository;
    public FindItemCategoryHandler( SynonymousWordsParser parser ,  ICatalogRepository catalogRepository )
    {
        _catalogRepository = catalogRepository;
        _parser = parser;
    }
    public async Task<FoundItemsResponce> Handle(FindItemCategoryRequestCommand request, CancellationToken cancellationToken)
    {
        var items = await _catalogRepository.GetItemsByCategoryAsync(request.request,request.page , request.pageSize);
        return new FoundItemsResponce(items);
    }
}