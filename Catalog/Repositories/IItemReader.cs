

using Catalog.Models;

public interface IItemReader
{
    Task<bool> GetItemAsync(Guid itemId , Guid userId);
    Task<Items> GetItemDataAsync(Guid itemId );
}