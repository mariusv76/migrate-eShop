using eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;

namespace eShop.Ordering.UnitTests.Domain;

[TestClass]
public class PaymentMethodTests
{
    [TestMethod]
    public void Create_Valid_PaymentMethod_Succeeds()
    {
        var exp = DateTime.UtcNow.AddMonths(1);
        var pm = new PaymentMethod(1, "alias", "4111111111111111", "123", "John Doe", exp);
        Assert.IsTrue(pm.IsEqualTo(1, "4111111111111111", exp));
    }

    [TestMethod]
    public void Create_Invalid_EmptyCardNumber_Throws()
    {
        Assert.ThrowsException<OrderingDomainException>(() => new PaymentMethod(1, "a", "", "123", "Name", DateTime.UtcNow.AddMonths(1)));
    }

    [TestMethod]
    public void Create_Invalid_EmptySecurityNumber_Throws()
    {
        Assert.ThrowsException<OrderingDomainException>(() => new PaymentMethod(1, "a", "4111", "", "Name", DateTime.UtcNow.AddMonths(1)));
    }

    [TestMethod]
    public void Create_Invalid_EmptyHolder_Throws()
    {
        Assert.ThrowsException<OrderingDomainException>(() => new PaymentMethod(1, "a", "4111", "123", "", DateTime.UtcNow.AddMonths(1)));
    }

    [TestMethod]
    public void Create_Invalid_PastExpiration_Throws()
    {
        Assert.ThrowsException<OrderingDomainException>(() => new PaymentMethod(1, "a", "4111", "123", "Name", DateTime.UtcNow.AddDays(-1)));
    }

    [TestMethod]
    public void IsEqualTo_False_When_Different()
    {
        var exp = DateTime.UtcNow.AddMonths(2);
        var pm = new PaymentMethod(1, "alias", "4111111111111111", "123", "John Doe", exp);
        Assert.IsFalse(pm.IsEqualTo(2, "4111111111111111", exp));
        Assert.IsFalse(pm.IsEqualTo(1, "4222222222222", exp));
        Assert.IsFalse(pm.IsEqualTo(1, "4111111111111111", exp.AddDays(1)));
    }
}
