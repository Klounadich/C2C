using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using Catalog.Services.BLOB;
using MediatR;

namespace Catalog.Handlers;

public class AddItemHandler :IRequestHandler<AddItemCommand,bool>
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly IBLOBbyService _blobyService;

    public AddItemHandler(ICatalogRepository catalogRepository , IBLOBbyService blobyService)
    {
        _catalogRepository = catalogRepository;
        _blobyService = blobyService;
    }
    public async Task<bool> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        var Item = new PutItemData
        {
            UserId = request.UserId,
            itemName = request.itemName,
            category = request.category,
            price = request.price,
            city = request.city,
            
        };
        Item.image = await _blobyService.UploadFile(request.image, Item.ItemId);
       return await _catalogRepository.AddItemAsync(Item);
        
    }
}