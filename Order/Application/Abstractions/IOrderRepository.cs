using Order.Application.Commands.CreateOrder;

namespace Order.Application.Abstractions;

public interface IOrderRepository
{
    public Task<CreateOrderResponce> CreateOrderAsync(CreateOrderCommand request);
}