using eShop.Ordering.API.Application.Commands;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.UnitTests.Application;

[TestClass]
public class CancelAndShipOrderCommandHandlerTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();

    private Order CreateNewOrder()
    {
        return new Order(
            userId: "user1",
            userName: "tester",
            address: new Address("street","city","state","country","zip"),
            cardTypeId: 1,
            cardNumber: "123456789012",
            cardSecurityNumber: "123",
            cardHolderName: "tester",
            cardExpiration: DateTime.UtcNow.AddYears(1));
    }

    [TestMethod]
    public async Task Cancel_OrderNotFound_ReturnsFalse()
    {
        var handler = new CancelOrderCommandHandler(_orderRepository);
        _orderRepository.GetAsync(Arg.Any<int>()).Returns((Order)null!);
        var result = await handler.Handle(new CancelOrderCommand(10), CancellationToken.None);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task Cancel_OrderFound_SetsCancelled_And_Saves()
    {
        var handler = new CancelOrderCommandHandler(_orderRepository);
        var order = CreateNewOrder(); // Submitted status
        _orderRepository.GetAsync(Arg.Any<int>()).Returns(order);
        _orderRepository.UnitOfWork.SaveEntitiesAsync(default).Returns(Task.FromResult(true));

        var result = await handler.Handle(new CancelOrderCommand(11), CancellationToken.None);

        Assert.IsTrue(result);
        Assert.AreEqual(OrderStatus.Cancelled, order.OrderStatus);
        await _orderRepository.UnitOfWork.Received(1).SaveEntitiesAsync(default);
    }

    [TestMethod]
    public async Task Ship_OrderNotFound_ReturnsFalse()
    {
        var handler = new ShipOrderCommandHandler(_orderRepository);
        _orderRepository.GetAsync(Arg.Any<int>()).Returns((Order)null!);
        var result = await handler.Handle(new ShipOrderCommand(15), CancellationToken.None);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task Ship_OrderFound_SetsShipped_And_Saves()
    {
        var handler = new ShipOrderCommandHandler(_orderRepository);
        var order = CreateNewOrder();
        // Move order through required states: Submitted -> AwaitingValidation -> StockConfirmed -> Paid
        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        order.SetPaidStatus();
        _orderRepository.GetAsync(Arg.Any<int>()).Returns(order);
        _orderRepository.UnitOfWork.SaveEntitiesAsync(default).Returns(Task.FromResult(true));

        var result = await handler.Handle(new ShipOrderCommand(16), CancellationToken.None);

        Assert.IsTrue(result);
        Assert.AreEqual(OrderStatus.Shipped, order.OrderStatus);
        await _orderRepository.UnitOfWork.Received(1).SaveEntitiesAsync(default);
    }
}
