using Cart.Models;

namespace Cart.Repositories;

public interface ICartRepository
{
    Task<Carts?> GetCartAsync(Guid userId);

    Task<bool> SameItemAsync(
        Guid cartId,
        Guid itemId);

    Task CreateCartAsync(Carts cart);

    Task AddItemToCartAsync(CartItems cartItem);
    Task<List<CartItems>> GetCartItemsAsync(Guid userId ,Guid cartId);

    Task SaveChangesAsync();
}