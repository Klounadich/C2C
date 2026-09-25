
using Cart.DTO;
using Cart.Models;
using Cart.Repositories;
using MediatR;
using Shared.Responce;

namespace Cart.Handlers;

public class AddItemHandler : IRequestHandler<AddItemDTO, Result>
{
    private readonly ICartRepository _cartRepository;
    private readonly IItemReader _itemReader;

    public AddItemHandler(
        ICartRepository cartRepository,
        IItemReader itemReader)
    {
        _cartRepository = cartRepository;
        _itemReader = itemReader;
    }

    public async Task<Result> Handle(
        AddItemDTO request,
        CancellationToken cancellationToken)
    {
        
        var item = await _itemReader.GetItemAsync(
            request.itemId,
            Guid.Parse(request.userId));

        if (!item)
        {
            return Result.Failure(
                ErrorType.ItemUnavailable,
                "Item is unavailable");
        }
        
        var cart = await _cartRepository.GetCartAsync(
            Guid.Parse(request.userId));
        if (cart == null)
        {
            cart = new Carts
            {
                id = Guid.NewGuid(),
                userId = Guid.Parse(request.userId)
            };

            await _cartRepository.CreateCartAsync(cart);
        }

        
        if (await _cartRepository.SameItemAsync(
                cart.id,
                request.itemId))
        {
            return Result.Failure(
                ErrorType.AlreadyInCart,
                "Item is already in cart");
        }
        
       

       
        var cartItem = new CartItems
        {
            id = Guid.NewGuid(),
            cart_id = cart.id,
            item_id = request.itemId,
            added_at = DateTime.UtcNow
        };

        await _cartRepository.AddItemToCartAsync(cartItem);

        try
        {
            await _cartRepository.SaveChangesAsync();
        }
        catch
        {
            return Result.Failure(
                ErrorType.DatabaseError,
                "Failed to add item to cart");
        }

        return Result.Success();
    }
}