using MoreCoffee.ViewModels;

namespace MoreCoffee;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel viewModel;
    
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await viewModel.OnNavigatedToAsync();
    }

    protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        await viewModel.OnNavigatedFromAsync();
    }
}

