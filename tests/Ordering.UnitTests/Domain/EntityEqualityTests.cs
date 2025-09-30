using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;
using eShop.Ordering.Domain.Seedwork;

namespace eShop.Ordering.UnitTests.Domain;

[TestClass]
public class EntityEqualityTests
{
    private sealed class TestEntity : Entity
    {
        public TestEntity(int id) { Id = id; }
    }

    [TestMethod]
    public void Entities_With_Same_Id_And_Type_Are_Equal()
    {
        var a = new TestEntity(5);
        var b = new TestEntity(5);
        Assert.IsTrue(a == b);
        Assert.AreEqual(a, b);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Entities_With_Different_Id_Are_Not_Equal()
    {
        var a = new TestEntity(1);
        var b = new TestEntity(2);
        Assert.IsTrue(a != b);
        Assert.AreNotEqual(a, b);
    }

    [TestMethod]
    public void Null_Entity_Comparisons_Work()
    {
        TestEntity a = null;
        var b = new TestEntity(3);
        Assert.IsTrue(a != b);
        Assert.IsTrue(b != a);
        Assert.IsFalse(b == a);
    }

    [TestMethod]
    public void Reference_Equality_Short_Circuit()
    {
        var a = new TestEntity(7);
        var b = a;
        Assert.IsTrue(a == b);
    }

    [TestMethod]
    public void Different_Types_Same_Id_Not_Equal()
    {
        var order = new Order("user","name", new Address("street","city","state","country","zip"), 1, "4111","123","name", DateTime.UtcNow.AddYears(1));
        var buyer = new Buyer(Guid.NewGuid().ToString(), "buyer");
        typeof(Entity).GetProperty("Id")!.SetValue(buyer, order.Id);
        Assert.AreNotEqual<object>(order, buyer);
        Assert.IsTrue(order != buyer);
    }
}
