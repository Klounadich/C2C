using MediatR;
using Order.Application.Abstractions;
using Order.Infrastructure.Repositories;
using Payments.Application.Abstractions;
using Payments.Domain;

namespace Payments.Application.Commands.CreatePayment;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand,CreatePaymentResponce>

{
    private readonly OrderReader _orderReader;
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentHandler(OrderReader orderReader , IPaymentRepository paymentRepository)
    {
        _orderReader = orderReader;
        _paymentRepository = paymentRepository;
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
                //send to provider
                var link = "temp";
                return new CreatePaymentResponce(payment.Id, link , "");
            }
        }
        return null;
    }    
}