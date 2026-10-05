namespace Order.Application.Queries.GetOrders;

public record GetOrdersResponce(
    List<OrderData> orders
    );
    
    public record OrderData(Guid orderId,
        string status,
        decimal amount,
        DateTime createdAt,
        List<Guid> productIds);