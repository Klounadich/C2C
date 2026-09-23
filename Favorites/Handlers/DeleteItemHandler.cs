using Favorites.DTOs;
using Favorites.Repositories;
using MediatR;
using Shared.Responce;

namespace Favorites.Handlers;

public class DeleteItemHandler :IRequestHandler<DeleteItemDTO , Result>
{
    private readonly IFavoritesRepository _favoritesRepository;

    public DeleteItemHandler(IFavoritesRepository favoritesRepository)
    {
        _favoritesRepository = favoritesRepository;
    }
    public async Task<Result> Handle(DeleteItemDTO request, CancellationToken cancellationToken)
    {
        if (await _favoritesRepository.RemoveFavoriteAsync(request.userId, request.itemId))
            return Result.Success();
        return Result.Failure(errorType:ErrorType.DatabaseError , "failed to delete favorite");
    }
}