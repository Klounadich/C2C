using Catalog.DTO;
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

    public async Task<List<Items>> GetItemsAsync(
        List<SearchTerm> terms,
        int page,
        int pageSize)
    {
        if (terms == null || terms.Count == 0)
        {
            return await _catalogDBContext.Items
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        IQueryable<Items> query = _catalogDBContext.Items;

        foreach (var term in terms)
        {
            var alternatives = term.Alternatives;

            query = query.Where(item =>
                alternatives.Any(keyword =>
                    item.title.ToLower().Contains(keyword)));
        }

        return await query
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Items>> GetItemsByCategoryAsync(
        string category,
        int page,
        int pageSize)
    {
        if (category == "rnd")
        {
            return await _catalogDBContext.Items.OrderBy(x=>x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        else
        {
            return await _catalogDBContext.Items
                .Where(x => x.category == category)
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }

    public async Task<bool> AddItemAsync(PutItemData request)
    {
        var put = new Items
        {
            UserId =  request.UserId,
            Id =   request.ItemId,
            category = request.category,
            title = request.itemName,
            price = request.price,
            city =  request.city,
            img_link = request.image
            
        };
        await _catalogDBContext.Items.AddAsync(put);
        return await _catalogDBContext.SaveChangesAsync() > 0;

    }
}