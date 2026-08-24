using Catalog.Models;

namespace Catalog.Repositories;

public interface ICatalogRepository
{
    public Task<List<Items>> GetItemsAsync(List<string> keywords , int page, int pageSize);
    public Task<List<Items>> GetItemsByCategoryAsync(string category , int page , int pageSize);
}