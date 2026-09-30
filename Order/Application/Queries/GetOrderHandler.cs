using MediatR;
using Order.Application.Abstractions;
using Order.Controllers.Contracts;
using Order.Domain;

namespace Order.Application.Queries;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, Orders>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Orders> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
      return await  _orderRepository.GetOrder(request);
        
    }
}