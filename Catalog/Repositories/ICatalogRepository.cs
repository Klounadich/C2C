using Catalog.Commands;
using Catalog.DTO;
using Catalog.Models;

namespace Catalog.Repositories;

public interface ICatalogRepository
{
    public Task<List<Items>> GetItemsAsync(List<SearchTerm> terms, int page, int pageSize);
    public Task<List<Items>> GetItemsByCategoryAsync(string category , int page , int pageSize);
    public Task<bool> AddItemAsync(PutItemData request);
}