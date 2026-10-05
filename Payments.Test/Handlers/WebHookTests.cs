using Moq;
using Order.Application.Abstractions;
using Payments.Application.Abstractions;
using Payments.Application.Commands.WebHook;
using Payments.Controllers.Contracts;
using Payments.Domain;

namespace Payments.Test.Handlers;

public class WebHookTests
{
    private readonly Mock<IOrderReader> _orderReaderMock;
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly WebHookHandler _handler;
    
    public WebHookTests()
    {
        _orderReaderMock = new Mock<IOrderReader>();
        _paymentRepositoryMock = new Mock<IPaymentRepository>();
        

        _handler = new WebHookHandler(
            _paymentRepositoryMock.Object,
            _orderReaderMock.Object
            );
    }

    [Fact]
    public async Task Handle_WhenPaymentSucceededAndOrderUpdated_ReturnsTrue()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        var command = new PaymentWebhookRequest(
            paymentId,
            PaymentStatus.Succeeded);

        var statusResponse = new SetStatusResponce
        {
            orderId = orderId,
            status = PaymentStatus.Succeeded
        };

        _paymentRepositoryMock
            .Setup(x => x.SetStatus(command))
            .ReturnsAsync(statusResponse);

        _orderReaderMock
            .Setup(x => x.SetOrderStatus(orderId))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(result);

        _paymentRepositoryMock.Verify(
            x => x.SetStatus(command),
            Times.Once);

        _orderReaderMock.Verify(
            x => x.SetOrderStatus(orderId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPaymentNotSucceeded_ReturnsFalse()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        var command = new PaymentWebhookRequest(
            paymentId,
            PaymentStatus.Failed);

        var statusResponse = new SetStatusResponce
        {
            orderId = orderId,
            status = PaymentStatus.Failed
        };

        _paymentRepositoryMock
            .Setup(x => x.SetStatus(command))
            .ReturnsAsync(statusResponse);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(result);

        _paymentRepositoryMock.Verify(
            x => x.SetStatus(command),
            Times.Once);

        _orderReaderMock.Verify(
            x => x.SetOrderStatus(It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPaymentSucceededButOrderUpdateFails_ReturnsFalse()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        var command = new PaymentWebhookRequest(
            paymentId,
            PaymentStatus.Succeeded);

        var statusResponse = new SetStatusResponce
        {
            orderId = orderId,
            status = PaymentStatus.Succeeded
        };

        _paymentRepositoryMock
            .Setup(x => x.SetStatus(command))
            .ReturnsAsync(statusResponse);

        _orderReaderMock
            .Setup(x => x.SetOrderStatus(orderId))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(result);

        _paymentRepositoryMock.Verify(
            x => x.SetStatus(command),
            Times.Once);

        _orderReaderMock.Verify(
            x => x.SetOrderStatus(orderId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenPaymentNotFound_ReturnsFalse()
    {
        // Arrange
        var paymentId = Guid.NewGuid();

        var command = new PaymentWebhookRequest(
            paymentId,
            PaymentStatus.Succeeded);

        _paymentRepositoryMock
            .Setup(x => x.SetStatus(command))
            .ReturnsAsync((SetStatusResponce?)null);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(result);

        _paymentRepositoryMock.Verify(
            x => x.SetStatus(command),
            Times.Once);

        _orderReaderMock.Verify(
            x => x.SetOrderStatus(It.IsAny<Guid>()),
            Times.Never);
    }
        
}