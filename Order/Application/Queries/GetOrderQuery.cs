using MediatR;
using Order.Domain;

namespace Order.Application.Queries;

public record GetOrderQuery(
    Guid OrderId,
    Guid UserId): IRequest<Orders>;