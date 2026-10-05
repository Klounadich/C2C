using Moq;
using Order.Application.Abstractions;
using Payments.Application.Abstractions;
using Payments.Application.Commands.CancelPayment;

namespace Payments.Test.Handlers;

public class CancelPaymentTests
{
    private readonly Mock<IOrderReader> _orderReaderMock;
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;

    private readonly CancelPaymentHandler _handler;

    public CancelPaymentTests()
    {
        _orderReaderMock = new Mock<IOrderReader>();
        _paymentRepositoryMock = new Mock<IPaymentRepository>();

        _handler = new CancelPaymentHandler(
            _paymentRepositoryMock.Object,
            _orderReaderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenOrderCancelSucceedsAndPaymentCancelSucceeds_ReturnsTrue()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new CancelPaymentCommand(
            orderId,
            userId);

        _orderReaderMock
            .Setup(x => x.OrderCancel(orderId, userId))
            .ReturnsAsync(true);

        _paymentRepositoryMock
            .Setup(x => x.Cancel(command))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(result);

        _orderReaderMock.Verify(
            x => x.OrderCancel(orderId, userId),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Cancel(command),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenOrderCancelFails_ReturnsFalse()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new CancelPaymentCommand(
            orderId,
            userId);

        _orderReaderMock
            .Setup(x => x.OrderCancel(orderId, userId))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(result);

        _orderReaderMock.Verify(
            x => x.OrderCancel(orderId, userId),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Cancel(command),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenOrderCancelSucceedsButPaymentCancelFails_ReturnsFalse()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new CancelPaymentCommand(
            orderId,
            userId);

        _orderReaderMock
            .Setup(x => x.OrderCancel(orderId, userId))
            .ReturnsAsync(true);

        _paymentRepositoryMock
            .Setup(x => x.Cancel(command))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(result);

        _orderReaderMock.Verify(
            x => x.OrderCancel(orderId, userId),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Cancel(command),
            Times.Once);
    }
}