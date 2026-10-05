using Order.Application.Commands.CreateOrder;
using Order.Application.Queries;
using Order.Application.Queries.GetOrders;
using Order.Domain;

namespace Order.Application.Abstractions;

public interface IOrderRepository
{
    public Task<CreateOrderResponce> CreateOrderAsync(CreateOrderCommand request);
    public Task<Orders> GetOrder(GetOrderQuery query);
    public Task<GetOrdersResponce> GetOrdersAsync(GetOrdersQuery query);
}