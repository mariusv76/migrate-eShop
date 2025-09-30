using eShop.Ordering.API.Application.IntegrationEvents.EventHandling;
using eShop.Ordering.API.Application.IntegrationEvents.Events;
using eShop.Ordering.API.Application.Commands;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Ordering.API.Application.DomainEventHandlers;
using eShop.Ordering.API.Application.IntegrationEvents; // for IOrderingIntegrationEventService
using eShop.Ordering.Domain.Events;

namespace eShop.Ordering.UnitTests.IntegrationEvents;

[TestClass]
public class OtherIntegrationEventHandlerTests
{
    [TestMethod]
    public async Task OrderStockRejectedIntegrationEventHandler_Sends_SetStockRejected_Command_WithRejectedIds()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<OrderStockRejectedIntegrationEventHandler>>();
        var handler = new OrderStockRejectedIntegrationEventHandler(mediator, logger);
        var evt = new OrderStockRejectedIntegrationEvent(77, new List<ConfirmedOrderStockItem>
        {
            new ConfirmedOrderStockItem(1, true),
            new ConfirmedOrderStockItem(2, false),
            new ConfirmedOrderStockItem(3, false)
        });

        await handler.Handle(evt);

        await mediator.Received(1).Send(Arg.Is<SetStockRejectedOrderStatusCommand>(c => c.OrderNumber == 77 && c.OrderStockItems.SequenceEqual(new []{2,3})));
    }

    [TestMethod]
    public async Task GracePeriodConfirmedIntegrationEventHandler_Sends_SetAwaitingValidation_Command()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<GracePeriodConfirmedIntegrationEventHandler>>();
        var handler = new GracePeriodConfirmedIntegrationEventHandler(mediator, logger);
        var evt = new GracePeriodConfirmedIntegrationEvent(101);

        await handler.Handle(evt);

        await mediator.Received(1).Send(Arg.Is<SetAwaitingValidationOrderStatusCommand>(c => c.OrderNumber == 101));
    }

    [TestMethod]
    public async Task OrderShippedDomainEventHandler_Publishes_Integration_Event()
    {
        var orderRepository = Substitute.For<IOrderRepository>();
        var buyerRepository = Substitute.For<IBuyerRepository>();
        var integrationSvc = Substitute.For<IOrderingIntegrationEventService>();
        var logger = Substitute.For<ILogger<OrderShippedDomainEventHandler>>();

        var order = new Order("user","name", new Address("street","city","state","country","zip"), 1, "4111","123","name", DateTime.UtcNow.AddYears(1));
        order.SetPaymentMethodVerified(10, 20);
        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        order.SetPaidStatus();
        order.SetShippedStatus();

        orderRepository.GetAsync(order.Id).Returns(order);
        var buyer = new Buyer("identity-guid","BuyerName");
        buyerRepository.FindByIdAsync(10).Returns(buyer);

        var handler = new OrderShippedDomainEventHandler(orderRepository, logger, buyerRepository, integrationSvc);

        await handler.Handle(new OrderShippedDomainEvent(order), CancellationToken.None);

        await integrationSvc.Received(1).AddAndSaveEventAsync(Arg.Is<OrderStatusChangedToShippedIntegrationEvent>(e => e.OrderId == order.Id));
    }
}
