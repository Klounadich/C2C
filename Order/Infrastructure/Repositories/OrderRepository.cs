using Catalog.Models;
using Order.Application.Abstractions;
using Order.Application.Commands.CreateOrder;
using Order.Domain;
using Order.Infrastructure.Persistance;

namespace Order.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDBContext _context;
    private readonly IItemReader _itemReader;

    public OrderRepository(OrderDBContext context ,  IItemReader itemReader)
    {
        _context = context;
        _itemReader = itemReader;
    }

    public async Task<CreateOrderResponce> CreateOrderAsync(CreateOrderCommand request)
    {
        var order = new Orders
        {
            userId = request.userId,
            ProductIds =request.ProductIds,
            Amount = request.amount,
            
        };
        await _context.AddAsync(order);
        if (await _context.SaveChangesAsync() > 0)
        {
            List<Items> items = new List<Items>();
            foreach (var item in request.ProductIds)
            {
               var itemdata = await _itemReader.GetItemDataAsync(item);
                itemdata.status = "reserved";
                items.Add(itemdata);
            }
            return new CreateOrderResponce(order.id);
        }
        return new CreateOrderResponce(Guid.Empty);
    }
}