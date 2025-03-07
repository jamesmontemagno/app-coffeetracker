using Microsoft.Maui.Controls;

namespace MoreCoffee.Services;

public static class PageService
{
    public static Page? GetCurrentPage()
    {
        var window = Application.Current?.Windows?.FirstOrDefault();
        return window?.Page as Page;
    }
}