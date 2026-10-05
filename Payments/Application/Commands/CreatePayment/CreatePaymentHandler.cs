using MediatR;
using Order.Application.Abstractions;
using Order.Infrastructure.Repositories;
using Payments.Application.Abstractions;
using Payments.Domain;
using Payments.Services;

namespace Payments.Application.Commands.CreatePayment;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand,CreatePaymentResponce>

{
    private readonly IOrderReader _orderReader;
    private readonly IProviderService _providerService;
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentHandler(IOrderReader orderReader , IPaymentRepository paymentRepository , IProviderService providerService)
    {
        _orderReader = orderReader;
        _paymentRepository = paymentRepository;
        _providerService = providerService;
    }
    public async Task<CreatePaymentResponce> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var existing = await _paymentRepository
            .GetByIdempotencyKey(
                request.idempotencyKey,
                cancellationToken);

        if (existing != null)
            return existing;
        
        var order = await _orderReader.GetOrder(request.OrderId , request.UserId);
        if (order != null)
        {
            var payment = new Payment
            {
                OrderId = request.OrderId,
                UserId = request.UserId,
                Amount = order.Amount,
                Currency = order.currency,
                Status = PaymentStatus.Pending,
                Provider = "",
                IdempotencyKey = request.idempotencyKey

            };
            if (await _paymentRepository.Create(payment, cancellationToken))
            {
                
                var link = await _providerService.SendPayment( payment.Id,payment.Amount, payment.Currency, "Оплата заказа TradeHub");
                return new CreatePaymentResponce(payment.Id, link , "");
            }
        }
        return new CreatePaymentResponce(Guid.Empty, "", "failed to create payment");
    }    
}