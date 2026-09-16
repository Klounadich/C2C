using Catalog.Commands;
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
    public async Task<bool> UpdateItemDataAsync(PutItemData request)
    {
        var item = await _catalogDBContext.Items
            .Where(x => x.Id == request.ItemId && x.UserId == request.UserId)
            .FirstOrDefaultAsync();
        if (item is null) return false; 
   
        item.category = request.category;
        item.title = request.itemName;
        item.price = request.price;
        item.city = request.city;
        item.img_link = request.image ?? item.img_link;
        item.moderated = false;
        item.updated_at = DateTime.UtcNow;
       
        return await _catalogDBContext.SaveChangesAsync() >0 ;
    }
    public async Task<List<Items>> GetItemsAsync(
        List<SearchTerm> terms,
        int page,
        int pageSize)
    {
        if (terms == null || terms.Count == 0)
        {
            return await _catalogDBContext.Items
                .Where(x => x.moderated)
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        IQueryable<Items> query = _catalogDBContext.Items
            .Where(x => x.moderated);
        foreach (var term in terms)
        {
            var alternatives = term.Alternatives;
            query = query.Where(item =>
                alternatives.Any(keyword =>
                    EF.Functions.Like(item.title.ToLower(), "% " + keyword + " %")
                    || item.title.ToLower() == keyword
                    || EF.Functions.Like(item.title.ToLower(), keyword + " %")
                    || EF.Functions.Like(item.title.ToLower(), "% " + keyword)));
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
            return await _catalogDBContext.Items
                .Where(x => x.moderated)
                .OrderBy(x=>x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        else
        {
            return await _catalogDBContext.Items
                .Where(x => x.moderated && x.category == category)
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
            img_link = request.image,
            moderated = false 
        };
        await _catalogDBContext.Items.AddAsync(put);
        return await _catalogDBContext.SaveChangesAsync() > 0;
    }

    

    

    public async Task SaveLog(SearchLogs logs)
    {
        await _catalogDBContext.SearchLogs.AddAsync(logs);
        await _catalogDBContext.SaveChangesAsync();
    }
}