using Moq;
using Order.Application.Abstractions;
using Order.Domain;
using Order.Infrastructure.Repositories;
using Payments.Application.Abstractions;
using Payments.Application.Commands.CreatePayment;
using Payments.Domain;
using Payments.Services;

namespace Payments.Test.Handlers;

public class CreatePaymentHandlerTests
{
    private readonly Mock<IOrderReader> _orderReaderMock;
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly Mock<IProviderService> _providerServiceMock;

    private readonly CreatePaymentHandler _handler;

    public CreatePaymentHandlerTests()
    {
        _orderReaderMock = new Mock<IOrderReader>();
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
        _providerServiceMock = new Mock<IProviderService>();

        _handler = new CreatePaymentHandler(
            _orderReaderMock.Object,
            _paymentRepositoryMock.Object,
            _providerServiceMock.Object);
    }
    
    [Fact]
    public async Task Handle_WhenOrderExists_CreatesPaymentAndReturnsUrl()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new CreatePaymentCommand(
            orderId,
            userId,
            "test-key");

        var order = new Orders
        {
            id = orderId,
            Amount = 100,
            currency = "RUB"
        };

        _paymentRepositoryMock
            .Setup(x => x.GetByIdempotencyKey(
                "test-key",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreatePaymentResponce?)null);

        _orderReaderMock
            .Setup(x => x.GetOrder(orderId, userId))
            .ReturnsAsync(order);

        _paymentRepositoryMock
            .Setup(x => x.Create(
                It.IsAny<Payment>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _providerServiceMock
            .Setup(x => x.SendPayment(
                It.IsAny<Guid>(),
                100,
                "RUB",
                "Оплата заказа TradeHub"))
            .ReturnsAsync("https://payment.test/123");

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("https://payment.test/123", result.PaymentUrl);

        _paymentRepositoryMock.Verify(
            x => x.Create(
                It.Is<Payment>(p =>
                    p.OrderId == orderId &&
                    p.UserId == userId &&
                    p.Amount == 100 &&
                    p.Currency == "RUB" &&
                    p.Status == PaymentStatus.Pending),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _providerServiceMock.Verify(
            x => x.SendPayment(
                It.IsAny<Guid>(),
                100,
                "RUB",
                "Оплата заказа TradeHub"),
            Times.Once);
    }
    
    
    [Fact]
    public async Task Handle_WhenOrderDoesntExist_DoesNotCreatePayment()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new CreatePaymentCommand(
            orderId,
            userId,
            "test-key");
        

        _paymentRepositoryMock
            .Setup(x => x.GetByIdempotencyKey(
                "test-key",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreatePaymentResponce?)null);

        _orderReaderMock
            .Setup(x => x.GetOrder(orderId, userId))
            .ReturnsAsync((Orders?)null);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Guid.Empty, result.PaymentId);
        Assert.Equal("", result.PaymentUrl);
        Assert.NotEqual("", result.ErrorMessage);

        _paymentRepositoryMock.Verify(
            x => x.Create(
                It.IsAny<Payment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _providerServiceMock.Verify(
            x => x.SendPayment(
                It.IsAny<Guid>(),
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenIdempotencyKeyExists_ReturnsExistingPayment()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var command = new CreatePaymentCommand(
            orderId,
            userId,
            "test-key");
        
        var payment = new CreatePaymentResponce(Guid.NewGuid(),"https://payment.test/123" , "");
        _paymentRepositoryMock
            .Setup(x => x.GetByIdempotencyKey(
                "test-key",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);
        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);
        //Assert
        Assert.NotNull(result);
        Assert.Equal("https://payment.test/123", result.PaymentUrl);
        
        _paymentRepositoryMock.Verify(
            x => x.Create(
                It.IsAny<Payment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _orderReaderMock.Verify(
            x => x.GetOrder(
                It.IsAny<Guid>(),
                It.IsAny<Guid>()),
            Times.Never);

        _providerServiceMock.Verify(
            x => x.SendPayment(
                It.IsAny<Guid>(),
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPaymentCreationFails_ReturnsFailure()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new CreatePaymentCommand(
            orderId,
            userId,
            "test-key");

        var order = new Orders
        {
            id = orderId,
            Amount = 100,
            currency = "RUB"
        };

        _paymentRepositoryMock
            .Setup(x => x.GetByIdempotencyKey(
                "test-key",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreatePaymentResponce?)null);

        _orderReaderMock
            .Setup(x => x.GetOrder(orderId, userId))
            .ReturnsAsync(order);

        _paymentRepositoryMock
            .Setup(x => x.Create(
                It.IsAny<Payment>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Guid.Empty, result.PaymentId);
        Assert.Equal("", result.PaymentUrl);
        Assert.NotEqual("", result.ErrorMessage);

        _paymentRepositoryMock.Verify(
            x => x.Create(
                It.IsAny<Payment>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _providerServiceMock.Verify(
            x => x.SendPayment(
                It.IsAny<Guid>(),
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenProviderFails_ReturnsFailure()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new CreatePaymentCommand(
            orderId,
            userId,
            "test-key");

        var order = new Orders
        {
            id = orderId,
            Amount = 100,
            currency = "RUB"
        };

        _paymentRepositoryMock
            .Setup(x => x.GetByIdempotencyKey(
                "test-key",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreatePaymentResponce?)null);

        _orderReaderMock
            .Setup(x => x.GetOrder(orderId, userId))
            .ReturnsAsync(order);

        _paymentRepositoryMock
            .Setup(x => x.Create(
                It.IsAny<Payment>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _providerServiceMock
            .Setup(x => x.SendPayment(
                It.IsAny<Guid>(),
                100,
                "RUB",
                "Оплата заказа TradeHub"))
            .ReturnsAsync("");

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("", result.PaymentUrl);

        _paymentRepositoryMock.Verify(
            x => x.Create(
                It.Is<Payment>(p =>
                    p.OrderId == orderId &&
                    p.UserId == userId &&
                    p.Amount == 100 &&
                    p.Currency == "RUB" &&
                    p.Status == PaymentStatus.Pending),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _providerServiceMock.Verify(
            x => x.SendPayment(
                It.IsAny<Guid>(),
                100,
                "RUB",
                "Оплата заказа TradeHub"),
            Times.Once);
    }
}