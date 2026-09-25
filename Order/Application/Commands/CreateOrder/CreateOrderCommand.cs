using MediatR;

namespace Order.Application.Commands.CreateOrder;

public record CreateOrderCommand(
    List<Guid> ProductIds,
    decimal amount,
    Guid userId
        
    ): IRequest<CreateOrderResponce>;