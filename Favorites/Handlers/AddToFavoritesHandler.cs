using Favorites.DTOs;
using Favorites.Repositories;
using MediatR;
using Shared.Responce;

namespace Favorites.Handlers;

public class AddToFavoritesHandler : IRequestHandler<AddItemDTO,Result>
{
    private readonly IFavoritesRepository _favoritesRepository;

    public AddToFavoritesHandler(IFavoritesRepository favoritesRepository)
    {
        _favoritesRepository = favoritesRepository;
    }
    public async Task<Result> Handle(AddItemDTO request, CancellationToken cancellationToken)
    {
        if (!await _favoritesRepository.SameItemAsync(request.userId, request.itemId))
        {
            if (await _favoritesRepository.AddFavoriteAsync(request.userId, request.itemId))
            {
                return Result.Success(); 
            }
            
        }

        return Result.Failure(errorType: ErrorType.AlreadyInCart, errorMessage: "Item is already in favorites");
    }
}