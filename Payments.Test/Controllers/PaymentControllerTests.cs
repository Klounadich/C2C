using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Payments.Application.Commands.CreatePayment;
using Payments.Application.Commands.RefundPayment;
using Payments.Application.Queries;
using Payments.Controllers;
using Payments.Controllers.Contracts;
using Payments.Domain;

namespace Payments.Tests.Controllers;

public class PaymentControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PaymentController _controller;

    public PaymentControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();

        _controller = new PaymentController(
            _mediatorMock.Object);
    }

    private void SetUser(Guid userId)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                userId.ToString())
        };

        var identity = new ClaimsIdentity(
            claims,
            "TestAuth");

        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = principal
            }
        };
    }

    [Fact]
    public async Task CreatePayment_ShouldReturnOk()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();

        SetUser(userId);

        var request = new CreatePaymentRequest(
            orderId,
            "test-idempotency-key");

        var response = new CreatePaymentResponce(
            paymentId,
            "https://payment.test/123",
            "");

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<CreatePaymentCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act

        var result = await _controller.CreatePayment(request);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<CreatePaymentCommand>(command =>
                    command.OrderId == orderId &&
                    command.UserId == userId &&
                    command.idempotencyKey == "test-idempotency-key"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPayment_ShouldReturnOk()
    {
        // Arrange

        var paymentId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var updatedAt = DateTime.UtcNow;

        var response = new GetPaymentResponce(
            paymentId,
            orderId,
            PaymentStatus.Pending,
            100m,
            "RUB",
            updatedAt);

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<GetPaymentQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act

        var result = await _controller.GetPayment(paymentId);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<GetPaymentQuery>(query =>
                    query.PaymentId == paymentId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task Webhook_ShouldReturnOk()
    {
        // Arrange

        var paymentId = Guid.NewGuid();

        var request = new PaymentWebhookRequest(
            paymentId,
            PaymentStatus.Pending);

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<PaymentWebhookRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act

        var result = await _controller.Webhook(request);

        // Assert

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(true, okResult.Value);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<PaymentWebhookRequest>(webhook =>
                    webhook.PaymentId == paymentId &&
                    webhook.status == PaymentStatus.Pending),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task RefundPayment_ShouldReturnOk()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();

        SetUser(userId);

        // Act

        var result = await _controller.RefundPayment(paymentId);

        // Assert

        Assert.IsType<OkObjectResult>(result);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<RefundPaymentCommand>(command =>
                    command.OrderId == paymentId &&
                    command.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    /*[Fact]
    public void CancelPayment_ShouldReturnOk()
    {
        // Arrange

        var paymentId = Guid.NewGuid();

        // Act

        var result = _controller.CancelPayment(paymentId);

        // Assert

        Assert.IsType<OkResult>(result);
    }
    */
}