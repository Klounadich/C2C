using Catalog.Models;
using Catalog.Repositories;

namespace Catalog.Services.SearchLogger;

public class SearchLogger
{
    private readonly ICatalogRepository _catalogRepository;

    public SearchLogger(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }
    
    public async Task Log(SearchLogs logs)
    {
       await _catalogRepository.SaveLog(logs);
    }
}