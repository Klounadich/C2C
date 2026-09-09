using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using Catalog.Services.BLOB;
using Catalog.Services.RabbitMQ;
using MediatR;

namespace Catalog.Handlers;

public class ChangeItemDataHandler : IRequestHandler<ChangeItemCommand , bool>
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly IRabbitMQService _rabbitMQService;
    private readonly IBLOBbyService _blobService;

    public ChangeItemDataHandler(ICatalogRepository catalogRepository , IRabbitMQService rabbitMQService , IBLOBbyService blobService)
    {
        _catalogRepository = catalogRepository;
        _rabbitMQService = rabbitMQService;
        _blobService = blobService;
    }
    public async Task<bool> Handle(ChangeItemCommand request, CancellationToken cancellationToken)
    {
        var item_data = new ItemModerationDTO
        {
            ItemId = request.ItemId,
            ItemName = request.itemName
        };
        if (request.image != null)
        {
            var img_link = await _blobService.UploadFile(request.image, request.ItemId);
        }
            
        var result = await _catalogRepository.UpdateItemDataAsync(new PutItemData
        {
            UserId = request.userId,
            ItemId = item_data.ItemId,
            itemName =  request.itemName,
            category = request.category,
            price = request.price,
            city =  request.city,
            
            
            
        });
        if (result)
        {
            await _rabbitMQService.SendMessageAsync(item_data); 
            
        }
        return result;
        
        
    }
}