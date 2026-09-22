using Cart.Commands;
using Cart.DTO;
using Cart.Repositories;
using MediatR;

namespace Cart.Handlers;

public class GetCartCountHandler : IRequestHandler<GetCartCountRequest, GetCartCountResponce>
{
    private readonly ICartRepository _cartRepository;

    public GetCartCountHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    public async Task<GetCartCountResponce> Handle(GetCartCountRequest request, CancellationToken cancellationToken)
    {
        return await _cartRepository.GetCartItemsCountAsync(request.userId);
    }
}