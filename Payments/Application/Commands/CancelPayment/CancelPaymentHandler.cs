using MediatR;
using Order.Application.Abstractions;
using Payments.Application.Abstractions;

namespace Payments.Application.Commands.CancelPayment;

public class CancelPaymentHandler : IRequestHandler<CancelPaymentCommand, bool>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderReader _orderReader;

    public CancelPaymentHandler(IPaymentRepository paymentRepository, IOrderReader orderReader)
    {
        _paymentRepository = paymentRepository;
        _orderReader = orderReader;
    }
    public async Task<bool> Handle(CancelPaymentCommand request, CancellationToken cancellationToken)
    {
        if(await _orderReader.OrderCancel(request.OrderId , request.UserId))
        {return await _paymentRepository.Cancel(request);}
       
        return false;
    }
}