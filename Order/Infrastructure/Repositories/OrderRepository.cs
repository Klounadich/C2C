using Order.Application.Abstractions;
using Order.Application.Commands.CreateOrder;
using Order.Domain;
using Order.Infrastructure.Persistance;

namespace Order.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDBContext _context;

    public OrderRepository(OrderDBContext context)
    {
        _context = context;
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
            return new CreateOrderResponce(order.id);
        }
        return new CreateOrderResponce(Guid.Empty);
    }
}