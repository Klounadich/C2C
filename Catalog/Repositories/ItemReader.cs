

using Catalog.Infrastructure;
using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Repositories;

public class ItemReader : IItemReader
{
    private readonly CatalogDBContext _dbContext;

    public ItemReader(CatalogDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> GetItemAsync(Guid itemId, Guid userId)
    {
        return await _dbContext.Items
            .Where(x => x.Id == itemId && x.UserId != userId && x.moderated && x.status == "sale").AnyAsync();
    }

    public async Task<Items> GetItemDataAsync(Guid itemId)
    {
        return await _dbContext.Items.Where(x => x.Id == itemId && x.moderated && x.status == "sale").FirstOrDefaultAsync();
    }

    public async Task ReserveAsync(List<Items> itemIds)
    {
        await _dbContext.SaveChangesAsync();
    }
} 