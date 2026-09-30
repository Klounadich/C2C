using Catalog.Models;
using Microsoft.EntityFrameworkCore;
using Order.Application.Abstractions;
using Order.Application.Commands.CreateOrder;
using Order.Application.Queries;
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
            await _itemReader.ReserveAsync(items);
            return new CreateOrderResponce(order.id);
        }
        return new CreateOrderResponce(Guid.Empty);
    }

    public async Task<Orders> GetOrder(GetOrderQuery query)
    {
        return await _context.Orders.Where(x => x.userId == query.UserId && x.id == query.OrderId).FirstOrDefaultAsync();
    }
        
}