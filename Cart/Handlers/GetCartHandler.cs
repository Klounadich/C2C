using Cart.Commands;
using Cart.DTO;
using Cart.Repositories;
using MediatR;
using Shared.Responce;

namespace Cart.Handlers;

public class GetCartHandler : IRequestHandler<GetCartRequest , CartResponce>
{
    private readonly ICartRepository  _repository;

    public GetCartHandler(ICartRepository repository)
    {
        _repository = repository;
    }
    public async  Task<CartResponce> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
        var cart = await _repository.GetCartAsync(request.userId);
        if ( cart != null)
        {
            return new CartResponce { items = await _repository.GetCartItemsAsync(request.userId, cart.id) };
        }
        return null;
        
    }
}