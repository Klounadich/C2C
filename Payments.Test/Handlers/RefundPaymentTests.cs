using Moq;
using Order.Application.Abstractions;
using Payments.Application.Abstractions;
using Payments.Application.Commands.RefundPayment;

namespace Payments.Test.Handlers;

public class RefundPaymentHandlerTests
{
    private readonly Mock<IOrderReader> _orderReaderMock;
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;

    private readonly RefundPaymentHandler _handler;

    public RefundPaymentHandlerTests()
    {
        _orderReaderMock = new Mock<IOrderReader>();
        _paymentRepositoryMock = new Mock<IPaymentRepository>();

        _handler = new RefundPaymentHandler(
            _orderReaderMock.Object,
            _paymentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenOrderRefundSucceedsAndPaymentRefundSucceeds_ReturnsTrue()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new RefundPaymentCommand(
            orderId,
            userId);

        _orderReaderMock
            .Setup(x => x.OrderRefund(orderId, userId))
            .ReturnsAsync(true);

        _paymentRepositoryMock
            .Setup(x => x.Refund(command))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(result);

        _orderReaderMock.Verify(
            x => x.OrderRefund(orderId, userId),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Refund(command),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenOrderRefundFails_ReturnsFalse()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new RefundPaymentCommand(
            orderId,
            userId);

        _orderReaderMock
            .Setup(x => x.OrderRefund(orderId, userId))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(result);

        _orderReaderMock.Verify(
            x => x.OrderRefund(orderId, userId),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Refund(command),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenOrderRefundSucceedsButPaymentRefundFails_ReturnsFalse()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new RefundPaymentCommand(
            orderId,
            userId);

        _orderReaderMock
            .Setup(x => x.OrderRefund(orderId, userId))
            .ReturnsAsync(true);

        _paymentRepositoryMock
            .Setup(x => x.Refund(command))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(result);

        _orderReaderMock.Verify(
            x => x.OrderRefund(orderId, userId),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Refund(command),
            Times.Once);
    }
}