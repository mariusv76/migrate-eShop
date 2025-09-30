using eShop.Ordering.API.Application.Commands;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.UnitTests.Application;

[TestClass]
public class SetAwaitingValidationOrderStatusCommandHandlerTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly SetAwaitingValidationOrderStatusCommandHandler _handler;
    private readonly Order _order;

    public SetAwaitingValidationOrderStatusCommandHandlerTests()
    {
        _handler = new SetAwaitingValidationOrderStatusCommandHandler(_orderRepository);
        _order = new Order(
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
    public async Task Handle_OrderNotFound_ReturnsFalse()
    {
        _orderRepository.GetAsync(Arg.Any<int>()).Returns((Order)null!);
        var result = await _handler.Handle(new SetAwaitingValidationOrderStatusCommand(5), CancellationToken.None);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task Handle_OrderFound_SetsAwaitingValidation_And_Saves()
    {
        _orderRepository.GetAsync(Arg.Any<int>()).Returns(_order);
        _orderRepository.UnitOfWork.SaveEntitiesAsync(default).Returns(Task.FromResult(true));

        var result = await _handler.Handle(new SetAwaitingValidationOrderStatusCommand(10), CancellationToken.None);

        Assert.IsTrue(result);
        Assert.AreEqual(OrderStatus.AwaitingValidation, _order.OrderStatus);
        await _orderRepository.UnitOfWork.Received(1).SaveEntitiesAsync(default);
    }
}
