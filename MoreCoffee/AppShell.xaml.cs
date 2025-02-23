namespace MoreCoffee;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("EditCoffeePage", typeof(Views.EditCoffeePage));
    }
}
