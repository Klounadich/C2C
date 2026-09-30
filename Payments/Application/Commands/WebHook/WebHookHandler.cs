using MediatR;
using Order.Infrastructure.Repositories;
using Payments.Application.Abstractions;
using Payments.Controllers.Contracts;
using Payments.Domain;

namespace Payments.Application.Commands.WebHook;

public class WebHookHandler: IRequestHandler<PaymentWebhookRequest, bool>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly OrderReader _reader;

    public WebHookHandler(IPaymentRepository paymentRepository, OrderReader reader)
    {
        _paymentRepository = paymentRepository;
        _reader = reader;
    }
    public async Task<bool> Handle(PaymentWebhookRequest request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.SetStatus(request);
        if (payment.status == PaymentStatus.Succeeded)
        {
           if (await _reader.SetOrderStatus(payment.orderId))
               return true;
        }
        return false;
    }
}