using MediatR;
using Order.Application.Abstractions;

namespace Order.Application.Commands.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponce>
{
    private readonly IOrderRepository _repository;

    public CreateOrderHandler(IOrderRepository repository)
    {
        _repository = repository;
    }
    public async Task<CreateOrderResponce> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var responce = await _repository.CreateOrderAsync(request);
        return responce;
    }
}