using CommunityToolkit.Mvvm.ComponentModel;
using MoreCoffee.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace MoreCoffee.ViewModels;

public partial class StatisticsViewModel : ObservableObject
{
    private readonly CoffeeService _coffeeService;

    [ObservableProperty]
    private string totalOuncesText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CoffeeDayData> chartData;

    [ObservableProperty]
    private ObservableCollection<CoffeeDayData> chartData2;

    [ObservableProperty]
    public ObservableCollection<CoffeeDayData> chartData3 = new ObservableCollection<CoffeeDayData>();

    public StatisticsViewModel(CoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
        ChartData = new ObservableCollection<CoffeeDayData>();
        ChartData2 = new ObservableCollection<CoffeeDayData>();
    }

    public async void OnAppearing()
    {
        await LoadStatisticsAsync();
    }

    private async Task LoadStatisticsAsync()
    {
        var coffees = await _coffeeService.GetCoffeesAsync();
        var totalOunces = coffees.Sum(c => c.Ounces);
        TotalOuncesText = $"{totalOunces:N1}";

        // Group coffees by date for the chart
        var groupedCoffees = coffees
            .GroupBy(c => c.DateAdded.Date)
            .OrderBy(g => g.Key)
            .Select(g => new CoffeeDayData(g.Key, g.Sum(c => c.Ounces)))
            .ToList();

        ChartData.Clear();
        foreach (var data in groupedCoffees.TakeLast(7)) // Show last 7 days
            ChartData.Add(data);
        ChartData2.Clear();
        //group the coffees by name for chartdata2
        var groupedCoffeesByName = coffees
            .GroupBy(c => c.Name)
            .OrderBy(g => g.Key)
            .Select(g => new CoffeeDayData(DateTime.Today, g.Sum(c => c.Ounces)))
            .ToList();
        foreach (var data in groupedCoffeesByName)
            ChartData2.Add(data);


        ChartData3.Clear();
        var groupedCoffeesByName2 = coffees
            .GroupBy(c => c.Name)
            .OrderBy(g => g.Key)
            .Select(g => new CoffeeDayData(DateTime.Today, g.Sum(c => c.Ounces)))
            .ToList();

        foreach (var data in groupedCoffeesByName2)
            ChartData3.Add(data);
    }
}

public class CoffeeDayData
{
    public DateTime Date { get; }
    public string DisplayDate => Date.ToString("M/d");
    public double Ounces { get; }

    public CoffeeDayData(DateTime date, double ounces)
    {
        Date = date;
        Ounces = ounces;
    }
}