using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Platform;
using MoreCoffee.Models;
using MoreCoffee.Services;

namespace MoreCoffee.ViewModels;

[QueryProperty(nameof(Coffee), "Coffee")]
public partial class EditCoffeeViewModel(CoffeeService coffeeService) : ObservableObject
{
    private readonly CoffeeService coffeeService = coffeeService;
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

        if (Coffee.Ounces <= 0)
        {
            var page = PageService.GetCurrentPage();
            if (page != null)
            {
                await page.DisplayAlert("Validation Error", "Ounces must be greater than zero.", "OK");
            }
            return;
        }

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

    [RelayCommand]
    async Task DeleteCoffee()
    {
        if (Coffee == null)
            return;

        var page = PageService.GetCurrentPage();
        if (page == null)
            return;

        bool answer = await page.DisplayAlert(
            "Delete Coffee",
            $"Are you sure you want to delete {Coffee.Name}?",
            "Yes", "No");

        if (answer)
        {
            await coffeeService.DeleteCoffeeAsync(Coffee);
            
            var parameters = new Dictionary<string, object>
            {
                { "IsDeleted", true }
            };
            await Shell.Current.GoToAsync("..", true, parameters);
        }
    }
}