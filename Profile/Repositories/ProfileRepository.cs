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
    
    
}