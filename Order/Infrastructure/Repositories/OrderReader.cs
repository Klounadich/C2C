using Microsoft.EntityFrameworkCore;
using Order.Application.Abstractions;
using Order.Domain;
using Order.Infrastructure.Persistance;

namespace Order.Infrastructure.Repositories;

public class OrderReader
{
    private readonly OrderDBContext _context;
    public OrderReader(OrderDBContext context)
    {
        _context = context;
    }

    public async Task<Orders> GetOrder(Guid orderId , Guid userId)
    {
        return await _context.Orders.Where(x=> x.id == orderId && x.status == "WaitingPayment" && x.userId == userId).FirstOrDefaultAsync();
    }
}