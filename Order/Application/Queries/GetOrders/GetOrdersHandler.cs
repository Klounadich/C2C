using MediatR;
using Order.Application.Abstractions;

namespace Order.Application.Queries.GetOrders;

public class GetOrdersHandler: IRequestHandler<GetOrdersQuery, GetOrdersResponce>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<GetOrdersResponce> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _orderRepository.GetOrdersAsync(request);
    }
}