using Cart.Contracts;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Repositories;

public class ItemReader : IItemReader
{
    private readonly CatalogDBContext _dbContext;

    public ItemReader(CatalogDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async  Task<bool> GetItemAsync(Guid itemId , Guid userId)
    {
        return await _dbContext.Items.AnyAsync(x => x.Id == itemId && x.UserId != userId);
    }
} 