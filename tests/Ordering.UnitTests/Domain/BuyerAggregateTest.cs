namespace eShop.Ordering.UnitTests.Domain;

[TestClass]
public class BuyerAggregateTest
{
    public BuyerAggregateTest()
    { }

    [TestMethod]
    public void Create_buyer_item_success()
    {
        //Arrange    
        var identity = new Guid().ToString();
        var name = "fakeUser";

        //Act 
        var fakeBuyerItem = new Buyer(identity, name);

        //Assert
        Assert.IsNotNull(fakeBuyerItem);
    }

    [TestMethod]
    public void Create_buyer_item_fail_identity()
    {
        //Arrange    
        var identity = string.Empty;
        var name = "fakeUser";

        //Act - Assert
        Assert.ThrowsException<ArgumentNullException>(() => new Buyer(identity, name));
    }

    [TestMethod]
    public void Create_buyer_item_fail_name()
    {
        //Arrange    
        var identity = Guid.NewGuid().ToString();
        var name = string.Empty;

        //Act - Assert
        Assert.ThrowsException<ArgumentNullException>(() => new Buyer(identity, name));
    }

    [TestMethod]
    public void add_payment_success()
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);
        var orderId = 1;
        var name = "fakeUser";
        var identity = new Guid().ToString();
        var fakeBuyerItem = new Buyer(identity, name);

        //Act
        var result = fakeBuyerItem.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration, orderId);

        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, fakeBuyerItem.PaymentMethods.Count());
    }

    [TestMethod]
    public void add_duplicate_payment_does_not_create_new_method()
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "alias";
        var cardNumber = "4111";
        var securityNumber = "123";
        var cardHolderName = "Holder";
        var expiration = DateTime.UtcNow.AddMonths(2);
        var buyer = new Buyer(Guid.NewGuid().ToString(), "buyer");
        var pm1 = buyer.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration, 1);
        var domainEventsAfterFirst = buyer.DomainEvents.Count;
        var pm2 = buyer.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration, 2);

        //Act - Assert
        Assert.AreSame(pm1, pm2, "Should return existing payment method instance");
        Assert.AreEqual(1, buyer.PaymentMethods.Count(), "Should still have single payment method");
        Assert.AreEqual(domainEventsAfterFirst + 1, buyer.DomainEvents.Count, "Duplicate verification still raises event");
    }

    [TestMethod]
    public void add_payment_with_different_expiration_creates_new()
    {
        //Arrange
        var buyer = new Buyer(Guid.NewGuid().ToString(), "buyer");
        var cardTypeId = 1;
        var alias = "alias";
        var cardNumber = "4111";
        var security = "123";
        var holder = "Holder";
        var exp1 = DateTime.UtcNow.AddMonths(1);
        var exp2 = exp1.AddMonths(1);

        //Act
        buyer.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, security, holder, exp1, 1);
        buyer.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, security, holder, exp2, 2);

        //Assert
        Assert.AreEqual(2, buyer.PaymentMethods.Count(), "Different expiration should yield distinct payment methods");
    }

    [TestMethod]
    public void create_payment_method_success()
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);

        //Act
        var result = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        //Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void create_payment_method_expiration_fail()
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(-1);

        //Act - Assert
        Assert.ThrowsException<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration));
    }

    [TestMethod]
    public void payment_method_isEqualTo()
    {
        //Arrange    
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.UtcNow.AddYears(1);

        //Act
        var fakePaymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);
        var result = fakePaymentMethod.IsEqualTo(cardTypeId, cardNumber, expiration);

        //Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Add_new_PaymentMethod_raises_new_event()
    {
        //Arrange    
        var alias = "fakeAlias";
        var orderId = 1;
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.UtcNow.AddYears(1);
        var expectedResult = 1;
        var name = "fakeUser";

        //Act 
        var fakeBuyer = new Buyer(Guid.NewGuid().ToString(), name);
        fakeBuyer.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, orderId);

        //Assert
        Assert.AreEqual(expectedResult, fakeBuyer.DomainEvents.Count);
    }
}
