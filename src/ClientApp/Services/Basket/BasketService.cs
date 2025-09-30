using eShop.ClientApp.Models.Basket;
using eShop.ClientApp.Services.FixUri;
using eShop.ClientApp.Services.Identity;
using eShop.ClientApp.Services.Settings;
using BasketItem = eShop.ClientApp.Models.Basket.BasketItem;

namespace eShop.ClientApp.Services.Basket;

public class BasketService : IBasketService, IDisposable
{
    private readonly IFixUriService _fixUriService;
    private readonly IIdentityService _identityService;
    private readonly ISettingsService _settingsService;

    // Mock storage for basket items
    private readonly List<BasketItem> _mockBasketItems = new();

    public BasketService(IIdentityService identityService, ISettingsService settingsService,
        IFixUriService fixUriService)
    {
        _identityService = identityService;
        _settingsService = settingsService;
        _fixUriService = fixUriService;
    }

    public IEnumerable<BasketItem> LocalBasketItems { get; set; }

    public async Task<CustomerBasket> GetBasketAsync()
    {
        // Mock implementation using local storage
        await Task.Delay(100); // Simulate network delay

        var basket = new CustomerBasket();
        
        foreach (var item in _mockBasketItems)
        {
            basket.AddItemToBasket(new BasketItem 
            { 
                ProductId = item.ProductId, 
                Quantity = item.Quantity,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                PictureUrl = item.PictureUrl
            });
        }

        _fixUriService.FixBasketItemPictureUri(basket?.Items);
        return basket;
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
    {
        // Mock implementation - store items locally
        await Task.Delay(50); // Simulate network delay

        _mockBasketItems.Clear();
        
        if (customerBasket?.Items != null)
        {
            foreach (var item in customerBasket.Items)
            {
                _mockBasketItems.Add(new BasketItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    PictureUrl = item.PictureUrl
                });
            }
        }

        return await GetBasketAsync();
    }

    public async Task ClearBasketAsync()
    {
        // Mock implementation - clear local storage
        await Task.Delay(50); // Simulate network delay
        _mockBasketItems.Clear();
    }

    public void Dispose()
    {
        // Nothing to dispose in mock implementation
        GC.SuppressFinalize(this);
    }

    ~BasketService()
    {
        Dispose();
    }
}
