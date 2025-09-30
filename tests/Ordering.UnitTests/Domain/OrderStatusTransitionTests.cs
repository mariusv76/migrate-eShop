using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.UnitTests.Domain;

[TestClass]
public class OrderStatusTransitionTests
{
    private Order NewOrder()
        => new Order("user","name", new Address("street","city","state","country","zip"), 1, "4111111111111111", "123", "name", DateTime.UtcNow.AddYears(1));

    [TestMethod]
    public void SetAwaitingValidation_OnlyFromSubmitted()
    {
        var order = NewOrder();
        order.SetAwaitingValidationStatus();
        Assert.AreEqual(OrderStatus.AwaitingValidation, order.OrderStatus);
        // Calling again should not change or add duplicate events
        var eventCount = order.DomainEvents.Count;
        order.SetAwaitingValidationStatus();
        Assert.AreEqual(eventCount, order.DomainEvents.Count); // no new event
    }

    [TestMethod]
    public void SetStockConfirmed_RequiresAwaitingValidation()
    {
        var order = NewOrder();
        order.SetStockConfirmedStatus(); // should be ignored (still Submitted)
        Assert.AreEqual(OrderStatus.Submitted, order.OrderStatus);
        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        Assert.AreEqual(OrderStatus.StockConfirmed, order.OrderStatus);
        Assert.IsTrue(order.Description!.Contains("confirmed"));
    }

    [TestMethod]
    public void SetPaid_RequiresStockConfirmed()
    {
        var order = NewOrder();
        order.SetPaidStatus(); // ignored
        Assert.AreEqual(OrderStatus.Submitted, order.OrderStatus);
        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        order.SetPaidStatus();
        Assert.AreEqual(OrderStatus.Paid, order.OrderStatus);
        Assert.IsTrue(order.Description!.Contains("payment"));
    }

    [TestMethod]
    public void SetShipped_RequiresPaid()
    {
        var order = NewOrder();
        Assert.ThrowsException<OrderingDomainException>(() => order.SetShippedStatus());
        order.SetAwaitingValidationStatus();
        order.SetStockConfirmedStatus();
        order.SetPaidStatus();
        order.SetShippedStatus();
        Assert.AreEqual(OrderStatus.Shipped, order.OrderStatus);
    }

    [TestMethod]
    public void Cancel_DisallowedFromPaidOrShipped()
    {
        var order = NewOrder();
        // Allowed from Submitted
        order.SetCancelledStatus();
        Assert.AreEqual(OrderStatus.Cancelled, order.OrderStatus);

        var order2 = NewOrder();
        order2.SetAwaitingValidationStatus();
        order2.SetStockConfirmedStatus();
        order2.SetPaidStatus();
        Assert.ThrowsException<OrderingDomainException>(() => order2.SetCancelledStatus());

        var order3 = NewOrder();
        order3.SetAwaitingValidationStatus();
        order3.SetStockConfirmedStatus();
        order3.SetPaidStatus();
        order3.SetShippedStatus();
        Assert.ThrowsException<OrderingDomainException>(() => order3.SetCancelledStatus());
    }

    [TestMethod]
    public void CancelWhenStockRejected_OnlyFromAwaitingValidation_SetsDescription()
    {
        var order = NewOrder();
        order.SetAwaitingValidationStatus();
        order.SetCancelledStatusWhenStockIsRejected(new []{ 42 }); // product id not in order -> empty description list
        Assert.AreEqual(OrderStatus.Cancelled, order.OrderStatus);
        Assert.IsTrue(order.Description!.Contains("don't have stock"));
    }
}
