using eShop.Ordering.Infrastructure;
using eShop.Ordering.Infrastructure.Repositories;
using eShop.Ordering.Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore;
using eShop.Ordering.Domain.AggregatesModel.OrderAggregate;
using eShop.Ordering.Domain.AggregatesModel.BuyerAggregate;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Ordering.UnitTests.Infrastructure;

[TestClass]
public class InfrastructureTests
{
    private static OrderingContext NewContext(IMediator mediator = null)
    {
        var options = new DbContextOptionsBuilder<OrderingContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return mediator == null ? new OrderingContext(options) : new OrderingContext(options, mediator);
    }

    [TestMethod]
    public async Task OrderRepository_Add_And_GetAsync_Works()
    {
        using var ctx = NewContext();
        var repo = new OrderRepository(ctx);
        var order = new Order("user","name", new Address("street","city","state","country","zip"), 1, "4111","123","holder", DateTime.UtcNow.AddYears(1));
        repo.Add(order);
        await ctx.SaveChangesAsync();

        var loaded = await repo.GetAsync(order.Id);
        Assert.IsNotNull(loaded);
        Assert.AreEqual(order.Id, loaded.Id);
        Assert.AreEqual(OrderStatus.Submitted, loaded.OrderStatus);
    }

    [TestMethod]
    public async Task BuyerRepository_Add_Find_Update_Works()
    {
        using var ctx = NewContext();
        var buyerRepo = new BuyerRepository(ctx);
        var buyer = new Buyer(Guid.NewGuid().ToString(), "buyer");
        buyer.VerifyOrAddPaymentMethod(1, "alias", "4111", "123", "holder", DateTime.UtcNow.AddYears(1), 1);
        buyerRepo.Add(buyer);
        await ctx.SaveChangesAsync();

        var found = await buyerRepo.FindAsync(buyer.IdentityGuid);
        Assert.IsNotNull(found);
        Assert.AreEqual(buyer.IdentityGuid, found.IdentityGuid);
        Assert.AreEqual(1, found.PaymentMethods.Count());

        buyer.VerifyOrAddPaymentMethod(1, "alias2", "4222", "456", "holder", DateTime.UtcNow.AddYears(1), 2);
        buyerRepo.Update(buyer);
        await ctx.SaveChangesAsync();

        var foundAgain = await buyerRepo.FindByIdAsync(buyer.Id);
        Assert.AreEqual(2, foundAgain.PaymentMethods.Count());
    }

    private sealed class CapturingMediator : IMediator
    {
        private static async IAsyncEnumerable<T> EmptyAsync<T>()
        {
            // Ensure method contains an await to avoid CS1998
            await Task.Yield();
            yield break;
        }
        public List<INotification> Published { get; } = new();
        public Task Publish(object notification, CancellationToken cancellationToken = default) { Published.Add((INotification)notification); return Task.CompletedTask; }
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification { Published.Add(notification); return Task.CompletedTask; }
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) => Task.FromResult(default(TResponse));
        public Task<object> Send(object request, CancellationToken cancellationToken = default) => Task.FromResult<object>(null);
        // Non-returning requests
        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest => Task.CompletedTask;
        public Task Send(IRequest request, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default) => EmptyAsync<TResponse>();
        public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = default) => EmptyAsync<object>();
        // Explicit ISender support
        Task<TResponse> ISender.Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken) => Send(request, cancellationToken);
        Task<object> ISender.Send(object request, CancellationToken cancellationToken) => Send(request, cancellationToken);
    }

    [TestMethod]
    public async Task OrderingContext_SaveEntities_Dispatches_DomainEvents()
    {
        var mediator = new CapturingMediator();
        using var ctx = NewContext(mediator);
        var order = new Order("user","name", new Address("street","city","state","country","zip"), 1, "4111","123","holder", DateTime.UtcNow.AddYears(1));
        ctx.Orders.Add(order);
        await ctx.SaveEntitiesAsync();
        Assert.IsTrue(mediator.Published.OfType<OrderStartedDomainEvent>().Any(), "Domain event not dispatched");
    }

    [TestMethod]
    public async Task RequestManager_Prevents_Duplicate_Ids()
    {
        using var ctx = NewContext();
        var manager = new RequestManager(ctx);
        var id = Guid.NewGuid();
        await manager.CreateRequestForCommandAsync<object>(id);
        Assert.IsTrue(await manager.ExistAsync(id));
        await Assert.ThrowsExceptionAsync<OrderingDomainException>(async () => await manager.CreateRequestForCommandAsync<object>(id));
    }
}
