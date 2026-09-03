using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using Catalog.Services.BLOB;
using Catalog.Services.RabbitMQ;
using MediatR;

namespace Catalog.Handlers;

public class AddItemHandler :IRequestHandler<AddItemCommand,bool>
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly IBLOBbyService _blobyService;
    private readonly IRabbitMQService _rabbitMQService;
    

    public AddItemHandler(ICatalogRepository catalogRepository , IBLOBbyService blobyService ,  IRabbitMQService rabbitMQService)
    {
        _catalogRepository = catalogRepository;
        _blobyService = blobyService;
        _rabbitMQService = rabbitMQService;
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
        _rabbitMQService.SendMessageAsync(new ItemModerationDTO
        {
            ItemId = Item.ItemId,
            ItemName = Item.itemName
        });
        Item.image = await _blobyService.UploadFile(request.image, Item.ItemId);
       return await _catalogRepository.AddItemAsync(Item);
        
    }
}