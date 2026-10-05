using MediatR;

namespace Order.Application.Queries.GetOrders;

public record GetOrdersQuery(
    Guid userId):IRequest<GetOrdersResponce>;