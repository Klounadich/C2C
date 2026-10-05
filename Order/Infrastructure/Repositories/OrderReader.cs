using Microsoft.EntityFrameworkCore;
using Order.Application.Abstractions;
using Order.Domain;
using Order.Infrastructure.Persistance;

namespace Order.Infrastructure.Repositories;

public class OrderReader : IOrderReader
{
    private readonly OrderDBContext _context;
    private readonly IItemReader _itemReader;
    public OrderReader(OrderDBContext context , IItemReader itemReader)
    {
        _context = context;
        _itemReader = itemReader;
    }

    public async Task<Orders> GetOrder(Guid orderId , Guid userId)
    {
        return await _context.Orders.Where(x=> x.id == orderId && x.status == "WaitingPayment" && x.userId == userId).FirstOrDefaultAsync();
    }
    
    public async Task<bool> OrderRefund(Guid orderId , Guid userId)
    {
        var order = await _context.Orders
            .Where(x => x.id == orderId
                        && x.status == "Paid"
                        && x.userId == userId)
            .FirstOrDefaultAsync();

        if (order == null)
            return false;

        order.status = "Refunded";
        order.refunded_at = DateTime.UtcNow;

        if (await _context.SaveChangesAsync() > 0)
        {
            return await _itemReader.ReturnItemsToSale(order.ProductIds);
        }

        return false;
    }


    public async Task<bool> SetOrderStatus(Guid orderId)
    {
        var order = _context.Orders.Where(x => x.id == orderId).FirstOrDefault();
        order.status ="Paid";
        await _context.SaveChangesAsync();
        return true;
    }
}