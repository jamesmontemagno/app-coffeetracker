using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoreCoffee.Models;
using MoreCoffee.Services;

namespace MoreCoffee.ViewModels;

[QueryProperty(nameof(Coffee), "Coffee")]
public partial class EditCoffeeViewModel : ObservableObject
{
    private readonly CoffeeService coffeeService;
    private string originalName = string.Empty;
    private double originalOunces;
    private DateTime originalDateAdded;
    
    [ObservableProperty]
    Coffee? coffee;

    partial void OnCoffeeChanged(Coffee? value)
    {
        if (value != null)
        {
            originalName = value.Name ?? string.Empty;
            originalOunces = value.Ounces;
            originalDateAdded = value.DateAdded;
        }
    }

    public EditCoffeeViewModel(CoffeeService coffeeService)
    {
        this.coffeeService = coffeeService;
    }

    private bool HasCoffeeChanged()
    {
        if (Coffee == null)
            return false;

        return !string.Equals(Coffee.Name ?? string.Empty, originalName) ||
               Math.Abs(Coffee.Ounces - originalOunces) > 0.001 ||
               Coffee.DateAdded != originalDateAdded;
    }

    [RelayCommand]
    async Task SaveCoffee()
    {
        if (Coffee == null)
            return;

        var hasCoffeeChanged = HasCoffeeChanged();
        if (hasCoffeeChanged)
        {
            await coffeeService.UpdateCoffeeAsync(Coffee);
        }

        var parameters = new Dictionary<string, object>
        {
            { "IsEdited", hasCoffeeChanged }
        };
        await Shell.Current.GoToAsync("..", true, parameters);
    }
}