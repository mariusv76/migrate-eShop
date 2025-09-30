using eShop.Ordering.API.Application.Commands;
using eShop.Ordering.API.Application.IntegrationEvents.EventHandling;
using eShop.Ordering.API.Application.IntegrationEvents.Events;

namespace eShop.Ordering.UnitTests.IntegrationEvents;

[TestClass]
public class OrderStockConfirmedIntegrationEventHandlerTests
{
    [TestMethod]
    public async Task Handler_Sends_SetStockConfirmed_Command()
    {
        var mediator = Substitute.For<IMediator>();
        var logger = Substitute.For<ILogger<OrderStockConfirmedIntegrationEventHandler>>();
        var handler = new OrderStockConfirmedIntegrationEventHandler(mediator, logger);
        var evt = new OrderStockConfirmedIntegrationEvent(123);

        await handler.Handle(evt);

        await mediator.Received(1).Send(Arg.Is<SetStockConfirmedOrderStatusCommand>(c => c.OrderNumber == 123));
    }
}
