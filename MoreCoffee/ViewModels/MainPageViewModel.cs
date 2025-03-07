using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoreCoffee.Models;
using MoreCoffee.Services;
using System.Collections.ObjectModel;

namespace MoreCoffee.ViewModels;

[QueryProperty(nameof(IsEdited), "IsEdited")]
public partial class MainPageViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly CoffeeService _coffeeService;
    private readonly Random _random = new Random();
    
    private readonly string[] _coffeeCelebrations = new[]
    {
        "☕ Life happens, coffee helps!\nOne more delicious cup to brighten your day!",
        "☕ Coffee: because adulting is hard!\nSip, savor, and conquer the day!",
        "In the symphony of morning,\nCoffee plays the sweetest note.\nA melody of warmth and comfort,\nAs precious moments float.",
        "☕ Dark as night, sweet as love,\nCoffee brings blessings from above!",
        "Through steam and brew,\nMy heart beats true.\nAnother cup of joy,\nJust for you!",
        "☕ Every cup of coffee is a hug for your brain!\nAnd you just added another one!",
        "Coffee whispers softly,\nIn aromatic prose.\nAnother cup of heaven,\nAs your collection grows!"
    };

    [ObservableProperty]
    private ObservableCollection<CoffeeGroup> coffeeGroups;

    [ObservableProperty]
    private string totalOuncesText = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string ounces = string.Empty;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private bool isEdited;

    public MainPageViewModel(CoffeeService coffeeService, SettingsService settingsService)
    {
        _coffeeService = coffeeService;
        CoffeeGroups = new ObservableCollection<CoffeeGroup>();
    }

    private async Task LoadCoffeesAsync()
    {
        var coffeeList = await _coffeeService.GetCoffeesAsync();
        
        // Group coffees by date
        var groups = coffeeList
            .GroupBy(c => c.DateAdded.Date)
            .OrderByDescending(g => g.Key)
            .Select(g => new CoffeeGroup(g.Key, g.ToList()))
            .ToList();


        CoffeeGroups = new ObservableCollection<CoffeeGroup>(groups);
        OnPropertyChanged(nameof(CoffeeGroups));

		var totalOunces = coffeeList.Sum(c => c.Ounces);
		TotalOuncesText = $"Total Coffee Consumed: {totalOunces}oz";

    }

    [RelayCommand]
    private async Task AddCoffeeAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Ounces))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter both name and ounces", "OK");
            return;
        }

        if (!double.TryParse(Ounces, out double ouncesValue))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter a valid number for ounces", "OK");
            return;
        }

        var coffee = new Coffee
        {
            Name = Name,
            Ounces = ouncesValue,
            DateAdded = SelectedDate
        };

        await _coffeeService.AddCoffeeAsync(coffee);
        
        // Show a random celebration message
        var celebrationMessage = _coffeeCelebrations[_random.Next(_coffeeCelebrations.Length)];
        await Shell.Current.DisplayAlert("Coffee Joy! ☕", celebrationMessage, "Thanks!");
        
        // Clear the entries
        Name = string.Empty;
        Ounces = string.Empty;

        // Reload the list
        await LoadCoffeesAsync();
    }

    [RelayCommand]
    async Task EditCoffee(Coffee coffee)
    {
        var parameters = new Dictionary<string, object>
        {
            { "Coffee", coffee }
        };
        await Shell.Current.GoToAsync("EditCoffeePage", parameters);
    }


    public async Task OnNavigatedToAsync()
    {
        var shouldRefresh = IsEdited || CoffeeGroups.Count == 0;

        if (shouldRefresh)
        {
            await LoadCoffeesAsync();
            IsEdited = false;
        }
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}

public class CoffeeGroup : ObservableCollection<Coffee>
{
    public DateTime Date { get; }
    public string DisplayDate { get; }
    public double TotalOunces { get; }

    public CoffeeGroup(DateTime date, List<Coffee> coffees) : base(coffees)
    {
        Date = date;
        DisplayDate = date.ToString("D");
        TotalOunces = coffees.Sum(c => c.Ounces);
    }
}