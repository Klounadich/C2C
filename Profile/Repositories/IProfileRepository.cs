using Catalog.Commands;
using Catalog.DTO;
using Profile.Models;

namespace Profile.Repositories;

public interface IProfileRepository
{
    public Task<List<Items>> GetItemsAsync(Guid userId, int page, int pageSize);
    public Task<bool> RemoveItemAsync(RemoveItemCommand request);
   
}