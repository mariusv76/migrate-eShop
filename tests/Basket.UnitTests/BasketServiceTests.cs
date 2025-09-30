using System.Security.Claims;
using eShop.Basket.API.Repositories;
using eShop.Basket.API.Grpc;
using eShop.Basket.API.Model;
using eShop.Basket.UnitTests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using BasketItem = eShop.Basket.API.Model.BasketItem;

namespace eShop.Basket.UnitTests;

[TestClass]
public class BasketServiceTests
{
    [TestMethod]
    public async Task GetBasketReturnsEmptyForNoUser()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var serverCallContext = TestServerCallContext.Create();
        serverCallContext.SetUserState("__HttpContext", new DefaultHttpContext());

        var response = await service.GetBasket(new GetBasketRequest(), serverCallContext);

        Assert.IsInstanceOfType<CustomerBasketResponse>(response);
        Assert.AreEqual(response.Items.Count(), 0);
    }

    [TestMethod]
    public async Task GetBasketReturnsItemsForValidUserId()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        List<BasketItem> items = [new BasketItem { Id = "some-id" }];
        mockRepository.GetBasketAsync("1").Returns(Task.FromResult(new CustomerBasket { BuyerId = "1", Items = items }));
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var serverCallContext = TestServerCallContext.Create();
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", "1")]));
        serverCallContext.SetUserState("__HttpContext", httpContext);

        var response = await service.GetBasket(new GetBasketRequest(), serverCallContext);

        Assert.IsInstanceOfType<CustomerBasketResponse>(response);
        Assert.AreEqual(response.Items.Count(), 1);
    }

    [TestMethod]
    public async Task GetBasketReturnsEmptyForInvalidUserId()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        List<BasketItem> items = [new BasketItem { Id = "some-id" }];
        mockRepository.GetBasketAsync("1").Returns(Task.FromResult(new CustomerBasket { BuyerId = "1", Items = items }));
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var serverCallContext = TestServerCallContext.Create();
        var httpContext = new DefaultHttpContext();
        serverCallContext.SetUserState("__HttpContext", httpContext);

        var response = await service.GetBasket(new GetBasketRequest(), serverCallContext);

        Assert.IsInstanceOfType<CustomerBasketResponse>(response);
        Assert.AreEqual(response.Items.Count(), 0);
    }

    [TestMethod]
    public async Task GetBasket_ReturnsMappedProductIdAndQuantity()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        List<BasketItem> items = [new BasketItem { ProductId = 42, Quantity = 5 }];
        mockRepository.GetBasketAsync("buyer").Returns(Task.FromResult(new CustomerBasket { BuyerId = "buyer", Items = items }));
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var ctx = TestServerCallContext.Create();
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", "buyer")]));
        ctx.SetUserState("__HttpContext", httpContext);

        var response = await service.GetBasket(new GetBasketRequest(), ctx);

        Assert.AreEqual(1, response.Items.Count);
        var mapped = response.Items.Single();
        Assert.AreEqual(42, mapped.ProductId);
        Assert.AreEqual(5, mapped.Quantity);
    }

    [TestMethod]
    public async Task GetBasket_RepositoryReturnsNull_ReturnsEmpty()
    {
        var mockRepository = Substitute.For<IBasketRepository>();
        mockRepository.GetBasketAsync("buyer").Returns(Task.FromResult<CustomerBasket>(null));
        var service = new BasketService(mockRepository, NullLogger<BasketService>.Instance);
        var ctx = TestServerCallContext.Create();
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", "buyer")]));
        ctx.SetUserState("__HttpContext", httpContext);

        var response = await service.GetBasket(new GetBasketRequest(), ctx);

        Assert.AreEqual(0, response.Items.Count);
    }
}
