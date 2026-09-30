using Payments.Domain;

namespace Payments.Application.Commands.WebHook;

public class SetStatusResponce
{
    public Guid  orderId { get; set; }
    public PaymentStatus status { get; set; }
}