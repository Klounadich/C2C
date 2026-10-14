namespace Order.Controllers.Contracts;

public record CreateOrderRequest(
    List<Guid> ProductIds,
    decimal amount,
    double lantitude,
    double longitude);