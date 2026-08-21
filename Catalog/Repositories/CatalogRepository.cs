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
        foreach (var keyword in keywords)
        {
            var Items = _catalogDBContext.Items.Where(x => x.title.Contains(keyword)).ToListAsync();
        }
        return await _catalogDBContext.Items.ToListAsync();
    }
}