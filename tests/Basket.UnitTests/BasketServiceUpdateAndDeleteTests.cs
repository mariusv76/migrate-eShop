using System.Security.Claims;
using eShop.Basket.API.Grpc;
using eShop.Basket.API.Model;
using eShop.Basket.API.Repositories;
using eShop.Basket.UnitTests.Helpers;
using Grpc.Core;
using Microsoft.Extensions.Logging.Abstractions;

namespace eShop.Basket.UnitTests;

[TestClass]
public class BasketServiceUpdateAndDeleteTests
{
    private static BasketService CreateService(IBasketRepository repo) => new(repo, NullLogger<BasketService>.Instance);

    private static (TestServerCallContext ctx, DefaultHttpContext http) CreateContextWithUser(string sub = null)
    {
        var ctx = TestServerCallContext.Create();
        var http = new DefaultHttpContext();
        if (!string.IsNullOrEmpty(sub))
        {
            http.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("sub", sub) }));
        }
        ctx.SetUserState("__HttpContext", http);
        return (ctx, http);
    }

    [TestMethod]
    public async Task UpdateBasket_Success_ReturnsMappedItems()
    {
        // Arrange
        var repo = Substitute.For<IBasketRepository>();
        var basketReturned = new CustomerBasket { BuyerId = "u1", Items = { new eShop.Basket.API.Model.BasketItem { ProductId = 10, Quantity = 3 } } };
        repo.UpdateBasketAsync(Arg.Any<CustomerBasket>()).Returns(basketReturned);
        var service = CreateService(repo);
        var (ctx, _) = CreateContextWithUser("u1");
        var request = new UpdateBasketRequest();
        request.Items.Add(new eShop.Basket.API.Grpc.BasketItem { ProductId = 10, Quantity = 3 });

        // Act
        var response = await service.UpdateBasket(request, ctx);

        // Assert
        Assert.AreEqual(1, response.Items.Count);
        Assert.AreEqual(10, response.Items[0].ProductId);
        Assert.AreEqual(3, response.Items[0].Quantity);
        await repo.Received(1).UpdateBasketAsync(Arg.Is<CustomerBasket>(b => b.BuyerId == "u1" && b.Items.Count == 1));
    }

    [TestMethod]
    public async Task UpdateBasket_RepositoryReturnsNull_ThrowsNotFound()
    {
        // Arrange
        var repo = Substitute.For<IBasketRepository>();
        repo.UpdateBasketAsync(Arg.Any<CustomerBasket>()).Returns((CustomerBasket)null);
        var service = CreateService(repo);
        var (ctx, _) = CreateContextWithUser("user");
        var request = new UpdateBasketRequest();
        request.Items.Add(new eShop.Basket.API.Grpc.BasketItem { ProductId = 1, Quantity = 1 });

        // Act / Assert
        var ex = await Assert.ThrowsExceptionAsync<RpcException>(() => service.UpdateBasket(request, ctx));
        Assert.AreEqual(StatusCode.NotFound, ex.StatusCode);
    }

    [TestMethod]
    public async Task UpdateBasket_Unauthenticated_ThrowsUnauthenticated()
    {
        // Arrange
        var repo = Substitute.For<IBasketRepository>();
        var service = CreateService(repo);
        var (ctx, _) = CreateContextWithUser(); // no user
        var request = new UpdateBasketRequest();

        // Act / Assert
        var ex = await Assert.ThrowsExceptionAsync<RpcException>(() => service.UpdateBasket(request, ctx));
        Assert.AreEqual(StatusCode.Unauthenticated, ex.StatusCode);
        await repo.DidNotReceive().UpdateBasketAsync(Arg.Any<CustomerBasket>());
    }

    [TestMethod]
    public async Task DeleteBasket_Authenticated_CallsRepository()
    {
        // Arrange
        var repo = Substitute.For<IBasketRepository>();
        repo.DeleteBasketAsync(Arg.Any<string>()).Returns(true);
        var service = CreateService(repo);
        var (ctx, _) = CreateContextWithUser("abc");

        // Act
        await service.DeleteBasket(new DeleteBasketRequest(), ctx);

        // Assert
        await repo.Received(1).DeleteBasketAsync("abc");
    }

    [TestMethod]
    public async Task DeleteBasket_Unauthenticated_ThrowsUnauthenticated()
    {
        // Arrange
        var repo = Substitute.For<IBasketRepository>();
        var service = CreateService(repo);
        var (ctx, _) = CreateContextWithUser();

        // Act / Assert
        var ex = await Assert.ThrowsExceptionAsync<RpcException>(() => service.DeleteBasket(new DeleteBasketRequest(), ctx));
        Assert.AreEqual(StatusCode.Unauthenticated, ex.StatusCode);
        await repo.DidNotReceive().DeleteBasketAsync(Arg.Any<string>());
    }
}
