using eShop.Ordering.API.Application.Commands;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.UnitTests.Application;

[TestClass]
public class SetStockRejectedOrderStatusCommandHandlerTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly SetStockRejectedOrderStatusCommandHandler _handler;
    private readonly Order _order;

    public SetStockRejectedOrderStatusCommandHandlerTests()
    {
        _handler = new SetStockRejectedOrderStatusCommandHandler(_orderRepository);
        _order = new Order(
            userId: "user1",
            userName: "tester",
            address: new Address("street","city","state","country","zip"),
            cardTypeId: 1,
            cardNumber: "123456789012",
            cardSecurityNumber: "123",
            cardHolderName: "tester",
            cardExpiration: DateTime.UtcNow.AddYears(1));
        _order.SetAwaitingValidationStatus();
    }

    [TestMethod]
    public async Task Handle_OrderNotFound_ReturnsFalse()
    {
        _orderRepository.GetAsync(Arg.Any<int>()).Returns((Order)null!);
        var cmd = new SetStockRejectedOrderStatusCommand(55, new List<int> { 1, 2 });
        var result = await _handler.Handle(cmd, CancellationToken.None);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task Handle_OrderFound_CancelsOrder_And_Saves()
    {
        _orderRepository.GetAsync(Arg.Any<int>()).Returns(_order);
        _orderRepository.UnitOfWork.SaveEntitiesAsync(default).Returns(Task.FromResult(true));
        var cmd = new SetStockRejectedOrderStatusCommand(77, new List<int> { 3, 4 });

        await _handler.Handle(cmd, CancellationToken.None);

        Assert.AreEqual(OrderStatus.Cancelled, _order.OrderStatus);
        await _orderRepository.UnitOfWork.Received(1).SaveEntitiesAsync(default);
    }
}
