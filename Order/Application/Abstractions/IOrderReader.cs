using Order.Domain;

namespace Order.Application.Abstractions;

public interface IOrderReader
{
    public  Task<Orders?> GetOrder(Guid orderId, Guid userId);
    public  Task<bool> OrderRefund(Guid orderId, Guid userId);
    public  Task<bool> OrderCancel(Guid orderId, Guid userId);
    public  Task<bool> SetOrderStatus(Guid orderId);
}