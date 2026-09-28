using MediatR;
using Payments.Application.Abstractions;

namespace Payments.Application.Queries;

public class GetPaymentHandler : IRequestHandler<GetPaymentQuery,GetPaymentResponce>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    public async Task<GetPaymentResponce> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
    {
        return await _paymentRepository.Get(request.PaymentId, cancellationToken);
    }
}