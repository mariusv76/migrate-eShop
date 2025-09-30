using eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;
using eShop.Ordering.Domain.SeedWork;

namespace eShop.Ordering.UnitTests.Domain.SeedWork;

[TestClass]
public class EnumerationTests
{
    [TestMethod]
    public void GetAll_Returns_All_Defined_Static_Instances()
    {
        var all = Enumeration.GetAll<CardType>().ToList();
        CollectionAssert.AreEquivalent(new[] { CardType.Amex, CardType.Visa, CardType.MasterCard }, all);
    }

    [TestMethod]
    public void FromValue_Finds_Correct_Instance()
    {
        var visa = Enumeration.FromValue<CardType>(2);
        Assert.AreSame(CardType.Visa, visa);
    }

    [TestMethod]
    public void FromDisplayName_Finds_Correct_Instance()
    {
        var mc = Enumeration.FromDisplayName<CardType>(nameof(CardType.MasterCard));
        Assert.AreSame(CardType.MasterCard, mc);
    }

    [TestMethod]
    public void AbsoluteDifference_Returns_Expected_Value()
    {
        var diff = Enumeration.AbsoluteDifference(CardType.Amex, CardType.MasterCard);
        Assert.AreEqual(Math.Abs(CardType.Amex.Id - CardType.MasterCard.Id), diff);
    }

    [TestMethod]
    public void Equality_And_HashCode_Work()
    {
        Assert.IsTrue(CardType.Amex.Equals(CardType.Amex));
        Assert.AreEqual(CardType.Amex.GetHashCode(), CardType.Amex.GetHashCode());
        Assert.IsFalse(CardType.Amex.Equals(CardType.Visa));
    }

    [TestMethod]
    public void CompareTo_Orders_By_Id()
    {
        var sorted = new[] { CardType.MasterCard, CardType.Amex, CardType.Visa }.OrderBy(c => c).ToList();
        CollectionAssert.AreEqual(new[] { CardType.Amex, CardType.Visa, CardType.MasterCard }, sorted);
    }

    [TestMethod]
    public void FromValue_Invalid_Throws()
    {
        Assert.ThrowsException<InvalidOperationException>(() => Enumeration.FromValue<CardType>(99));
    }

    [TestMethod]
    public void FromDisplayName_Invalid_Throws()
    {
        Assert.ThrowsException<InvalidOperationException>(() => Enumeration.FromDisplayName<CardType>("NotAType"));
    }
}
