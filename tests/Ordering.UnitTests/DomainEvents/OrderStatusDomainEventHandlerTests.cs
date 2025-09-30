using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.Domain.Events;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Ordering.API.Application.IntegrationEvents;
using eShop.Ordering.API.Application.IntegrationEvents.Events;

namespace eShop.Ordering.UnitTests.DomainEvents;

[TestClass]
public class OrderStatusDomainEventHandlerTests
{
    private Order CreateOrderWithItemAndBuyer(int buyerId = 10)
    {
        var order = new Order("user","name", new Address("street","city","state","country","zip"), 1, "4111","123","name", DateTime.UtcNow.AddYears(1), buyerId);
        order.AddOrderItem(10, "Prod", 5m, 0m, "pic", 2);
        return order;
    }

    [TestMethod]
    public async Task AwaitingValidation_Handler_Publishes_Integration_Event()
    {
        var order = CreateOrderWithItemAndBuyer();
        var orderRepository = Substitute.For<IOrderRepository>();
        var buyerRepository = Substitute.For<IBuyerRepository>();
        var integrationSvc = Substitute.For<IOrderingIntegrationEventService>();
        var logger = Substitute.For<ILogger<OrderStatusChangedToAwaitingValidationDomainEventHandler>>();

        var buyer = new Buyer("identity","BuyerName");
        orderRepository.GetAsync(order.Id).Returns(order);
        buyerRepository.FindByIdAsync(order.BuyerId.Value).Returns(buyer);

        var handler = new OrderStatusChangedToAwaitingValidationDomainEventHandler(orderRepository, logger, buyerRepository, integrationSvc);
        var domainEvent = new OrderStatusChangedToAwaitingValidationDomainEvent(order.Id, order.OrderItems);

        await handler.Handle(domainEvent, CancellationToken.None);

        await integrationSvc.Received(1).AddAndSaveEventAsync(Arg.Is<OrderStatusChangedToAwaitingValidationIntegrationEvent>(e => e.OrderId == order.Id));
    }

    [TestMethod]
    public async Task StockConfirmed_Handler_Publishes_Integration_Event()
    {
        var order = CreateOrderWithItemAndBuyer();
        var orderRepository = Substitute.For<IOrderRepository>();
        var buyerRepository = Substitute.For<IBuyerRepository>();
        var integrationSvc = Substitute.For<IOrderingIntegrationEventService>();
        var logger = Substitute.For<ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler>>();

        var buyer = new Buyer("identity","BuyerName");
        orderRepository.GetAsync(order.Id).Returns(order);
        buyerRepository.FindByIdAsync(order.BuyerId.Value).Returns(buyer);

        var handler = new OrderStatusChangedToStockConfirmedDomainEventHandler(orderRepository, buyerRepository, logger, integrationSvc);
        var domainEvent = new OrderStatusChangedToStockConfirmedDomainEvent(order.Id);

        await handler.Handle(domainEvent, CancellationToken.None);

        await integrationSvc.Received(1).AddAndSaveEventAsync(Arg.Is<OrderStatusChangedToStockConfirmedIntegrationEvent>(e => e.OrderId == order.Id));
    }
}
