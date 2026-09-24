namespace Payments.Domain;

public enum PaymentStatus
{
    Pending,

    RequiresAction,

    Succeeded,

    Failed,

    Cancelled,

    RefundPending,

    Refunded
}