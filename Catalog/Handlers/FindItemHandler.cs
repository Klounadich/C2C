using Catalog.Commands;
using Catalog.DTO;
using Catalog.Models;
using Catalog.Repositories;
using Catalog.Services;
using Catalog.Services.SearchLogger;
using MediatR;

public class FindItemHandler
    : IRequestHandler<FindItemRequestCommand, FoundItemsResponce>
{
   
    private readonly SearchNormalizer _normalizer;
    private readonly ICatalogRepository _catalogRepository;
    private readonly AliasSearcher _aliasSearcher;
    private readonly SearchLogger _searchLogs;

    public FindItemHandler(
        ICatalogRepository catalogRepository,
        SearchNormalizer normalizer ,
        AliasSearcher aliasSearcher ,
        SearchLogger searchLogs)
    {
        
        _catalogRepository = catalogRepository;
        _normalizer = normalizer;
        _aliasSearcher = aliasSearcher;
        _searchLogs = searchLogs;
        
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

        var searchLogsData = new SearchLogs
        {
            ResultCount = items.Count,
            UserId = request.userId,
            Query = request.request,
        };
       await _searchLogs.Log(searchLogsData);
        return new FoundItemsResponce(items);
    }
}