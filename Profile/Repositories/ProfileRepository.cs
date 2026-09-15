using Catalog.Commands;
using Catalog.DTO;
using Microsoft.EntityFrameworkCore;
using Profile.Infrastructure;
using Profile.Models;

namespace Profile.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly ProfileDBContext _profileDBContext;

    public ProfileRepository(ProfileDBContext profileDbContext)
    {
        _profileDBContext = profileDbContext;
    }
    public async Task<List<Items>> GetItemsAsync(Guid userId, int page, int pageSize)
    {
        return await _profileDBContext.Items
            .Where( x=> x.UserId == userId)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<bool> RemoveItemAsync(RemoveItemCommand request)
    {
        await _profileDBContext.Items.Where(x=> x.Id == request.ItemId && x.UserId == request.userId).ExecuteDeleteAsync();
        return await _profileDBContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> UpdateItemDataAsync(PutItemData request)
    {
        var item = await _profileDBContext.Items
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
       
        return await _profileDBContext.SaveChangesAsync() >0 ;
    }
}