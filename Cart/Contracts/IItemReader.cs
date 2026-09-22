namespace Cart.Contracts;

public interface IItemReader
{
    Task<bool> GetItemAsync(Guid itemId , Guid userId);
}