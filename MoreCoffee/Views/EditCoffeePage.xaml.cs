namespace MoreCoffee.Views;

public partial class EditCoffeePage : ContentPage
{
    public EditCoffeePage(ViewModels.EditCoffeeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}