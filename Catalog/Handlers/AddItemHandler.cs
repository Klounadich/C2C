using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using MediatR;

namespace Catalog.Handlers;

public class AddItemHandler :IRequestHandler<AddItemCommand,bool>
{
    private readonly ICatalogRepository _catalogRepository;

    public AddItemHandler(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }
    public async Task<bool> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        //image saved in blob storage -> responce the img_link
       return await _catalogRepository.AddItemAsync(new PutItemData
        {
            UserId = request.UserId,
            itemName = request.itemName,
            category = request.category,
            price = request.price,
            city = request.city,
            //image if have {img_link}
        });
        
    }
}