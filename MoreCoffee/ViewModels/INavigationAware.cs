namespace MoreCoffee.ViewModels;

public interface INavigationAwareAsync
{
    Task OnNavigatedToAsync();
    Task OnNavigatedFromAsync();
}