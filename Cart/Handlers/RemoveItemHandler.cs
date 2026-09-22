using Cart.Commands;
using Cart.DTO;
using Cart.Repositories;
using MediatR;
using Shared.Responce;

namespace Cart.Handlers;

public class RemoveItemHandler : IRequestHandler<DeleteItemDTO, Result>
{
    
    private readonly ICartRepository _repository;

    public RemoveItemHandler(ICartRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result> Handle(DeleteItemDTO request, CancellationToken cancellationToken)
    {
        if (await _repository.DeleteItemFromCart(request.cartId, request.itemId))
        {
            return Result.Success();
        }
        return Result.Failure(errorType: ErrorType.DatabaseError , errorMessage:"Failed to delete item");
    }
}