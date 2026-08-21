using Catalog.Models;

namespace Catalog.Repositories;

public interface ICatalogRepository
{
    public Task<List<Items>> GetItemsAsync(List<string> keywords);
}