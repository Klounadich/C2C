using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using Catalog.Services;
using MediatR;

public class FindItemHandler
    : IRequestHandler<FindItemRequestCommand, FoundItemsResponce>
{
   
    private readonly SearchNormalizer _normalizer;
    private readonly ICatalogRepository _catalogRepository;
    private readonly AliasSearcher _aliasSearcher;

    public FindItemHandler(
        ICatalogRepository catalogRepository,
        SearchNormalizer normalizer ,
        AliasSearcher aliasSearcher)
    {
        
        _catalogRepository = catalogRepository;
        _normalizer = normalizer;
        _aliasSearcher = aliasSearcher;
        
    }

    public async Task<FoundItemsResponce> Handle(
        FindItemRequestCommand request,
        CancellationToken cancellationToken)
    {
        var query = _normalizer.Normalize(request.request);

        var searchTerms = await _aliasSearcher.SearchAsync(
            query.Tokens,
            cancellationToken);

        var items = await _catalogRepository.GetItemsAsync(
            searchTerms,
            request.page,
            request.pageSize);

        return new FoundItemsResponce(items);
    }
}