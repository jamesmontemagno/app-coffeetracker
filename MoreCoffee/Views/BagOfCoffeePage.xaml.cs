using MoreCoffee.ViewModels;

namespace MoreCoffee.Views;

public partial class BagOfCoffeePage : ContentPage
{
    public BagOfCoffeePage(BagOfCoffeeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}