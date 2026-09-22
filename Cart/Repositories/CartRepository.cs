using Cart.DTO;
using Cart.Infrastructure;
using Cart.Models;
using Microsoft.EntityFrameworkCore;

namespace Cart.Repositories;

public class CartRepository : ICartRepository
{
    private readonly CartDBContext _dbContext;

    public CartRepository(CartDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Carts?> GetCartAsync(Guid userId)
    {
        return await _dbContext.Carts
            .FirstOrDefaultAsync(x => x.userId == userId);
    }

    public async Task<bool> SameItemAsync(
        Guid cartId,
        Guid itemId)
    {
        return await _dbContext.CartItems
            .AnyAsync(x =>
                x.cart_id == cartId &&
                x.item_id == itemId);
    }

    public async Task CreateCartAsync(Carts cart)
    {
        await _dbContext.Carts.AddAsync(cart);
    }

    public async Task AddItemToCartAsync(CartItems cartItem)
    {
        await _dbContext.CartItems.AddAsync(cartItem);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<CartItems>> GetCartItemsAsync(Guid userId, Guid cartId)
    {
       return await  _dbContext.CartItems.Where(X => X.cart_id == cartId).ToListAsync();
    }

    public async Task<bool> DeleteItemFromCart(Guid cartId, Guid itemId )
    {
        var deleted = await _dbContext.CartItems
            .Where(x => x.cart_id == cartId && x.item_id == itemId)
            .ExecuteDeleteAsync();

        return deleted > 0;
    }

    public async Task<GetCartCountResponce> GetCartItemsCountAsync(Guid userId)
    {
        var cart = await GetCartAsync(userId);
        var items = _dbContext.CartItems.Where(x=> x.cart_id == cart.id).Count();
        return new GetCartCountResponce
        {
            count = items
        };
    }
}