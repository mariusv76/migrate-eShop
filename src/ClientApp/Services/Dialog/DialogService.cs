namespace eShop.ClientApp.Services;

public class DialogService : IDialogService
{
    public Task ShowAlertAsync(string message, string title, string buttonLabel)
    {
        var mainPage = Application.Current?.Windows?.FirstOrDefault()?.Page;
        return mainPage?.DisplayAlert(title, message, buttonLabel) ?? Task.CompletedTask;
    }
}
