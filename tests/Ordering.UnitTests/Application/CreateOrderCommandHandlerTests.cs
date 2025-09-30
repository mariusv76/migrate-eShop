using eShop.Ordering.API.Application.Commands;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Ordering.API.Application.IntegrationEvents;
using eShop.Ordering.API.Application.IntegrationEvents.Events; // added

namespace eShop.Ordering.UnitTests.Application;

[TestClass]
public class CreateOrderCommandHandlerTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly IIdentityService _identityService = Substitute.For<IIdentityService>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IOrderingIntegrationEventService _integrationEventService = Substitute.For<IOrderingIntegrationEventService>();
    private readonly ILogger<CreateOrderCommandHandler> _logger = Substitute.For<ILogger<CreateOrderCommandHandler>>();

    private CreateOrderCommandHandler CreateHandler() => new(_mediator, _integrationEventService, _orderRepository, _identityService, _logger);

    private CreateOrderCommand CreateValidCommand()
    {
        var items = new List<BasketItem>
        {
            new BasketItem
            {
                ProductId = 1,
                ProductName = "Item1",
                UnitPrice = 10m,
                PictureUrl = "pic",
                Quantity = 2
            }
        };
        return new CreateOrderCommand(items, "user1", "tester", "city", "street", "state", "country", "zip", "123456789012", "tester", DateTime.UtcNow.AddYears(1), "123", 1);
    }

    [TestMethod]
    public async Task Handle_ValidOrder_PersistsAndReturnsTrue()
    {
        // Arrange
        var cmd = CreateValidCommand();
        _orderRepository.UnitOfWork.SaveEntitiesAsync(default).Returns(Task.FromResult(true));
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        _orderRepository.Received(1).Add(Arg.Any<Order>());
        await _integrationEventService.Received(1).AddAndSaveEventAsync(Arg.Any<OrderStartedIntegrationEvent>());
        await _orderRepository.UnitOfWork.Received(1).SaveEntitiesAsync(default);
    }

    [TestMethod]
    public async Task Handle_SaveFails_ReturnsFalse()
    {
        // Arrange
        var cmd = CreateValidCommand();
        _orderRepository.UnitOfWork.SaveEntitiesAsync(default).Returns(Task.FromResult(false));
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.IsFalse(result);
        _orderRepository.Received(1).Add(Arg.Any<Order>());
        await _integrationEventService.Received(1).AddAndSaveEventAsync(Arg.Any<OrderStartedIntegrationEvent>());
        await _orderRepository.UnitOfWork.Received(1).SaveEntitiesAsync(default);
    }
}
