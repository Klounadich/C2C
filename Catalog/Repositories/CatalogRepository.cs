using Catalog.Infrastructure;
using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Repositories;

public class CatalogRepository :ICatalogRepository
{
    private readonly CatalogDBContext _catalogDBContext;

    public CatalogRepository(CatalogDBContext catalogDBContext)
    {
        _catalogDBContext = catalogDBContext;
    }

    public async Task<List<Items>> GetItemsAsync(List<string> keywords)
    {
        if (keywords == null || keywords.Count == 0)
            return await _catalogDBContext.Items.ToListAsync();

        return await _catalogDBContext.Items
            .Where(x => keywords.Any(keyword => x.title.Contains(keyword)))
            .ToListAsync();
    }
}