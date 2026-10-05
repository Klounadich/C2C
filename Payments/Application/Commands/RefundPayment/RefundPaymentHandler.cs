using MediatR;
using Order.Application.Abstractions;
using Order.Infrastructure.Repositories;
using Payments.Application.Abstractions;

namespace Payments.Application.Commands.RefundPayment;

public class RefundPaymentHandler : IRequestHandler<RefundPaymentCommand,bool>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderReader _orderReader;
    

    public RefundPaymentHandler(IOrderReader orderReader , IPaymentRepository paymentRepository)
    {
        _orderReader = orderReader;
        _paymentRepository = paymentRepository;
    }
    public async Task<bool> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
       if(await _orderReader.OrderRefund(request.OrderId , request.UserId))
       {return await _paymentRepository.Refund(request);}
       
       return false;
    }
}