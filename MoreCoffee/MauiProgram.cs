using Microsoft.Extensions.Logging;
using MoreCoffee.Services;
using MoreCoffee.ViewModels;
using MoreCoffee.Views;
using Syncfusion.Maui.Core.Hosting;

namespace MoreCoffee;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<CoffeeService>();
        builder.Services.AddSingleton<BagOfCoffeeService>();
        
        // ViewModels
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<StatisticsViewModel>();
        builder.Services.AddTransient<EditCoffeeViewModel>();
        builder.Services.AddSingleton<BagOfCoffeeViewModel>();
        
        // Pages
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<StatisticsPage>();
        builder.Services.AddTransient<EditCoffeePage>();
        builder.Services.AddSingleton<BagOfCoffeePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
