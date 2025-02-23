using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoreCoffee.Models;
using MoreCoffee.Services;
using System.Collections.ObjectModel;

namespace MoreCoffee.ViewModels;

public partial class BagOfCoffeeViewModel : ObservableObject, INavigationAwareAsync
{
    private readonly BagOfCoffeeService _coffeeService;

    [ObservableProperty]
    private ObservableCollection<BagOfCoffee> coffeeBags;

    [ObservableProperty]
    private string roaster = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string origin = string.Empty;

    [ObservableProperty]
    private DateTime roastDate = DateTime.Today;

    [ObservableProperty]
    private RoastLevel roastLevel;

    [ObservableProperty]
    private string tastingNotes = string.Empty;

    [ObservableProperty]
    private string selectedRoastLevel = "Medium";

    public List<string> RoastLevels { get; } = Enum.GetNames<RoastLevel>().ToList();

    public BagOfCoffeeViewModel(BagOfCoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
        CoffeeBags = new ObservableCollection<BagOfCoffee>();
    }

    private async Task LoadCoffeeBagsAsync()
    {
        var bags = await _coffeeService.GetBagsOfCoffeeAsync();
        CoffeeBags.Clear();
        foreach (var bag in bags.OrderByDescending(b => b.DateAdded))
            CoffeeBags.Add(bag);
    }

    [RelayCommand]
    private async Task AddCoffeeBagAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Roaster))
            return;

        var bag = new BagOfCoffee
        {
            Name = Name,
            Roaster = Roaster,
            Origin = Origin,
            RoastDate = RoastDate,
            RoastLevel = Enum.Parse<RoastLevel>(SelectedRoastLevel),
            TastingNotes = TastingNotes
        };

        await _coffeeService.AddBagOfCoffeeAsync(bag);
        CoffeeBags.Insert(0, bag);

        // Reset the form
        Name = string.Empty;
        Roaster = string.Empty;
        Origin = string.Empty;
        TastingNotes = string.Empty;
        RoastDate = DateTime.Today;
        SelectedRoastLevel = "Medium";
    }

    [RelayCommand]
    async Task DeleteCoffeeBag(BagOfCoffee bag)
    {
        if (bag == null)
            return;

        await _coffeeService.DeleteBagOfCoffeeAsync(bag);
        CoffeeBags.Remove(bag);
    }

    public async Task OnNavigatedToAsync()
    {
        await LoadCoffeeBagsAsync();
    }

    public Task OnNavigatedFromAsync()
    {
        return Task.CompletedTask;
    }
}